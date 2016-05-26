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

        public override FaultTree read(StreamReader stream)
        {
            FaultTree ft = null;

            //Preprocess lines to remove .dot structures and split information into single lines 
            List<String> lines = stream.ReadToEnd().Split('\n').ToList();
            lines = lines.GetRange(1, lines.Count() - 3).SelectMany(l => l.Split(';')).Where(l => !l.Trim().Equals("")).ToList();

            //Create datastructures to store parsed tokens
            var symbols = from line in lines
                          from pattern in patternMatcher
                          where pattern.IsMatch(line)
                          select new { Token = patternMatcher.IndexOf(pattern), Information = pattern.Match(line)};

            //Group symbols be their token (allows for simplified FT construction)
            var symbolGroup = from symbol in symbols
                              orderby symbol.Token
                              group symbol by symbol.Token;

            Debug.Assert(symbolGroup.ElementAt((int)DotParseToken.DOT_ROOT).Count() == 1);

            //Extract different node types
            var root = symbolGroup.ElementAt((int)DotParseToken.DOT_ROOT).ElementAt(0);
            var terminals = from symbol in symbolGroup.ElementAt((int)DotParseToken.DOT_IDENTIFIER)
                            select (FaultTreeNode)new FaultTreeTerminalNode(int.Parse(symbol.Information.Groups["id"].Value), int.Parse(symbol.Information.Groups["label"].Value));

            var gates = from symbol in symbolGroup.ElementAt((int)DotParseToken.DOT_GATE)
                        select (FaultTreeNode)new FaultTreeGateNode(int.Parse(symbol.Information.Groups["id"].Value), symbol.Information.Groups["operator"].Value); //symbol.Information.Groups["id"].Value;

            //Union on all nodes
            var nodes = (from t in terminals select new { ID = t.ID, Node = t }).Union(from g in gates select new { ID = g.ID, Node = g }).OrderBy(n => n.ID);


            (from trans in symbolGroup.ElementAt((int)DotParseToken.DOT_TRANSITION)
             let f = nodes.ElementAt(int.Parse(trans.Information.Groups["from"].Value))
             let t = nodes.ElementAt(int.Parse(trans.Information.Groups["to"].Value))
             select new { From = f, To = t }).ToList().ForEach(t => t.From.Node.Childs.Add(t.To.Node));
            

            return ft;
        }

        public override void write(FaultTree ft, StreamWriter stream)
        {
            throw new NotImplementedException();
        }

        public override FaultTreeFormat getFormatToken()
        {
            return FaultTreeFormat.FAULT_TREE_DOT;
        }
    }
}
