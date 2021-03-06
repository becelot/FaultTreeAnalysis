﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DotFaultTreeEncoder.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FaultTreeAnalysis.FaultTree.MarkovChain;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree
{
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    internal class DotFaultTreeEncoder : IFaultTreeCodec
    {
        // The .dot pattern syntax
        private static readonly List<Regex> PatternMatcher = new List<Regex> {
            new Regex(@"(?<from>\d*)[^-]*-\>[^\d]*(?<to>\d*).*dir=none.*"),
			new Regex("(?<from>\\d*)[^-]*-\\>[^\\d]*(?<to>\\d*).*dir=forward.*label=\"(?<rate>.*)\".*"),
			new Regex(@"(?<id>\d*)[^\[]*\[shape=point.*"),
            new Regex("(?<id>\\d*)[^\\[]*\\[shape=box.*label=\"(?<operator>.*)\".*"),
            new Regex("(?<id>\\d*)[^\\[]*\\[shape=circle.*label=\"(?<label>[^\"]*)\".*"),
            new Regex("(?<id>\\d*)[^\\[]*\\[shape=circle.*label=\"(?<label>.*)\".*comment=\"lambda=(?<lambda>.*).*,.*mu=(?<mu>.*)\".*")
        };

		public override FaultTree Read(FileStream stream)
        {
            var sr = new StreamReader(stream);

            //Preprocess lines to remove .dot structures and split information into single lines 
            List<string> lines = sr.ReadToEnd().Split('\n').ToList();
            lines = lines.Where(line => !string.IsNullOrWhiteSpace(line)).ToList().GetRange(1, lines.Count - 2).SelectMany(l => l.Split(';')).Where(l => !l.Trim().Equals("")).ToList();

            //Create datastructures to store parsed tokens
            var symbols = from line in lines
                          from pattern in PatternMatcher
                          where pattern.IsMatch(line.Trim())
                          select new { Token = PatternMatcher.IndexOf(pattern), Information = pattern.Match(line.Trim())};

            //Group symbols be their token (allows for simplified FT construction)
            var symbolGroup = from symbol in symbols
                              orderby symbol.Token
                              group symbol by symbol.Token;

            Debug.Assert(symbolGroup.Count(g => g.Key == (int)DotParseToken.DOT_ROOT) == 1);

            //Extract different node types
            var rootNode = (from r in symbolGroup.FirstOrDefault(g => g.Key == (int)DotParseToken.DOT_ROOT)
                            select FaultTreeNodeFactory.GetInstance().CreateGateNode(int.Parse(r.Information.Groups["id"].Value), FaultTreeNodeFactory.FaultTreeGateOperator.FAULT_TREE_OPERATOR_AND) as FaultTreeNode).ToList();

            var terminals = (from symbol in symbolGroup.FirstOrDefault(g => g.Key == (int)DotParseToken.DOT_IDENTIFIER)
							 select new FaultTreeTerminalNode(int.Parse(symbol.Information.Groups["id"].Value), int.Parse(symbol.Information.Groups["label"].Value))).ToList();

            var gates = (from symbol in symbolGroup.FirstOrDefault(g => g.Key == (int)DotParseToken.DOT_GATE)
						 select FaultTreeNodeFactory.GetInstance().CreateGateNode(int.Parse(symbol.Information.Groups["id"].Value), symbol.Information.Groups["operator"].Value) as FaultTreeNode).ToList();

            //Union on all nodes
            var nodes = (from t in terminals select new {t.ID, Node = (FaultTreeNode)t }).Union(from g in gates select new {g.ID, Node = g }).Union(from r in rootNode select new {r.ID, Node = r }).OrderBy(n => n.ID).ToList();


            (from trans in symbolGroup.FirstOrDefault(g => g.Key == (int)DotParseToken.DOT_TRANSITION)
			 let f = nodes[int.Parse(trans.Information.Groups["from"].Value)]
             let t = nodes[int.Parse(trans.Information.Groups["to"].Value)]
             select new { From = f, To = t }).ToList().ForEach(trans => trans.From.Node.Childs.Add(trans.To.Node));

		    MarkovChain<FaultTreeTerminalNode> markovChain = new MarkovChain<FaultTreeTerminalNode>(terminals.Count);

            terminals.ForEach(terminal => markovChain.InitialDistribution[terminal] = 0.0d);

			if (symbolGroup.FirstOrDefault(g => g.Key == (int) DotParseToken.DOT_MARKOV_TRANSITION)?.Any() ?? false)
			{
				(from trans in symbolGroup.FirstOrDefault(g => g.Key == (int)DotParseToken.DOT_MARKOV_TRANSITION)
				 let f = nodes[int.Parse(trans.Information.Groups["from"].Value)].Node as FaultTreeTerminalNode
				 let t = nodes[int.Parse(trans.Information.Groups["to"].Value)].Node as FaultTreeTerminalNode
				 let r = trans.Information.Groups["rate"].Value
				 select new { From = f, To = t, Rate = r }).ToList().ForEach(trans => markovChain[trans.From, trans.To] = double.Parse(trans.Rate));
			}

		    if (symbolGroup.FirstOrDefault(g => g.Key == (int) DotParseToken.DOT_IMPLICIT_CHAIN)?.Any() ?? false)
		    {
		        int newLabel = terminals.Max(n => n.Label) + 1;
		        int newID = nodes.Max(n => n.ID) + 1;

		        var markovChainGroup = symbolGroup.FirstOrDefault(g => g.Key == (int) DotParseToken.DOT_IMPLICIT_CHAIN);
		        if (markovChainGroup != null)
		            foreach (var implicitChain in markovChainGroup)
		            {
		                var newNode = new FaultTreeTerminalNode(newID, newLabel);
		                var existingNode = terminals.First(t => t.ID == int.Parse(implicitChain.Information.Groups["id"].Value));

		                markovChain[newNode, existingNode] = double.Parse(implicitChain.Information.Groups["lambda"].Value);
		                markovChain[existingNode, newNode] = double.Parse(implicitChain.Information.Groups["mu"].Value);
		                markovChain.InitialDistribution[newNode] = 1.0d;
		            }
		    }


		    return new FaultTree(rootNode.ElementAt(0).Childs.ElementAt(0), markovChain);
        }

        public override void Write(FaultTree ft, FileStream stream)
        {
            throw new NotImplementedException();
        }

        public override FaultTreeFormat GetFormatToken()
        {
            return FaultTreeFormat.FAULT_TREE_DOT;
        }
    }
}
