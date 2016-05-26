using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree
{
    enum DotParseToken
    {
        DOT_TRANSITION = 0,
        DOT_ROOT = 1,
        DOT_GATE = 2,
        DOT_IDENTIFIER = 3,
        DOT_INVALID = 4
    }

    class DotFaultTreeEncoder : IFaultTreeCodec
    {
        // The .dot pattern syntax
        private static List<Regex> patternMatcher = new List<Regex> {
            new Regex(@"(?<from>\d*)[^-]*-\>[^\d]*(?<to>\d*).*"),
            new Regex(@"(?<id>\d*)[^\[]*\[shape=point.*"),
            new Regex("(?<id>\\d*)[^\\[]*\\[shape=box.*label=\"(?<operator>.*)\".*"),
            new Regex("(?<id>\\d*)[^\\[]*\\[shape=circle.*label=\"(?<label>.*)\".*")
        };

        public override FaultTree read(FileStream stream)
        {
            StreamReader sr = new StreamReader(stream);

            //Preprocess lines to remove .dot structures and split information into single lines 
            List<String> lines = sr.ReadToEnd().Split('\n').ToList();
            lines = lines.GetRange(1, lines.Count() - 3).SelectMany(l => l.Split(';')).Where(l => !l.Trim().Equals("")).ToList();

            //Create datastructures to store parsed tokens
            var symbols = from line in lines
                          from pattern in patternMatcher
                          where pattern.IsMatch(line.Trim())
                          select new { Token = patternMatcher.IndexOf(pattern), Information = pattern.Match(line.Trim())};

            //Group symbols be their token (allows for simplified FT construction)
            var symbolGroup = from symbol in symbols
                              orderby symbol.Token
                              group symbol by symbol.Token;

            Debug.Assert(symbolGroup.ElementAt((int)DotParseToken.DOT_ROOT).Count() == 1);

            //Extract different node types
            var rootNode = (from r in symbolGroup.ElementAt((int)DotParseToken.DOT_ROOT)
                           select new FaultTreeGateNode(int.Parse(r.Information.Groups["id"].Value), FaultTreeGateNode.FaultTreeGateOperator.FAULT_TREE_OPERATOR_EQUAL) as FaultTreeNode).ToList();

            var terminals = (from symbol in symbolGroup.ElementAt((int)DotParseToken.DOT_IDENTIFIER)
                            select new FaultTreeTerminalNode(int.Parse(symbol.Information.Groups["id"].Value), int.Parse(symbol.Information.Groups["label"].Value)) as FaultTreeNode).ToList();

            var gates = (from symbol in symbolGroup.ElementAt((int)DotParseToken.DOT_GATE)
                        select new FaultTreeGateNode(int.Parse(symbol.Information.Groups["id"].Value), symbol.Information.Groups["operator"].Value) as FaultTreeNode).ToList();

            //Union on all nodes
            var nodes = (from t in terminals select new { ID = t.ID, Node = t }).Union(from g in gates select new { ID = g.ID, Node = g }).Union(from r in rootNode select new { ID = r.ID, Node = r }).OrderBy(n => n.ID).ToList();


            (from trans in symbolGroup.ElementAt((int)DotParseToken.DOT_TRANSITION)
             let f = nodes[int.Parse(trans.Information.Groups["from"].Value)]
             let t = nodes[int.Parse(trans.Information.Groups["to"].Value)]
             select new { From = f, To = t }).ToList().ForEach(trans => trans.From.Node.Childs.Add(trans.To.Node));
            

            return new FaultTree(rootNode.ElementAt(0));
        }

        public override void write(FaultTree ft, FileStream stream)
        {
            throw new NotImplementedException();
        }

        public override FaultTreeFormat getFormatToken()
        {
            return FaultTreeFormat.FAULT_TREE_DOT;
        }
    }
}
