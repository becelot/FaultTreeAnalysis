using System;
using System.Collections.Generic;
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
        DOT_ROOT,
        DOT_GATE,
        DOT_IDENTIFIER,
        DOT_INVALID
    }

    class DotFaultTreeEncoder : IFaultTreeCodec
    {
        // The .dot pattern syntax
        private static List<Regex> patternMatcher = new List<Regex> {
            new Regex(@"(?<from>\d*)[^-]*-\>[^\d]*(?<to>\d*).*"),
            new Regex(@"(?<id>\d*)[^\[]*\[shape=point.*"),
            new Regex("(?<id>\\d*)[^\\[]*\\[shape=box.*label=\"(?<typ>.*)\".*"),
            new Regex("(?<id>\\d*)[^\\[]*\\[shape=circle.*label=\"(?<identifier>.*)\".*")
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
