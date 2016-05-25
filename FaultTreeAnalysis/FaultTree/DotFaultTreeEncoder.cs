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
        private static List<Regex> patternMatcher = new List<Regex> {
            new Regex(@"(?<from>\d*)[^-]*-\>[^\d]*(?<to>\d*).*"),
            new Regex(@"(?<id>\d*)[^\[]*\[shape=point.*"),
            new Regex("(?<id>\\d*)[^\\[]*\\[shape=box.*label=\"(?<typ>.*)\".*"),
            new Regex("(?<id>\\d*)[^\\[]*\\[shape=circle.*label=\"(?<identifier>.*)\".*")
        };

        private class Symbol
        {
            public DotParseToken Token { get; set; }
            public Match MatchingInformation { get; set; }

            public Symbol(DotParseToken token, Match info)
            {
                this.Token = token;
                this.MatchingInformation = info;
            }
        }


        private Symbol classify(string s)
        {
            for (int i = 0; i < patternMatcher.Count(); i++)
            {
                if (!patternMatcher.ElementAt(i).IsMatch(s)) { continue;  }
                Match m = patternMatcher.ElementAt(i).Match(s);

                return new Symbol((DotParseToken)i, m);
            }
            return new Symbol(DotParseToken.DOT_INVALID, null);
        }

        public override FaultTree read(StreamReader stream)
        {
            FaultTree ft = null;

            List<String> lines = stream.ReadToEnd().Split('\n').ToList();
            lines = lines.GetRange(1, lines.Count() - 3).SelectMany(l => l.Split(';')).Where(l => !l.Trim().Equals("")).ToList();
            


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
