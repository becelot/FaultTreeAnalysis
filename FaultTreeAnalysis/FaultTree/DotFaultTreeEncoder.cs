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
        DOT_TRANSITION,
        DOT_ROOT,
        DOT_GATE,
        DOT_IDENTIFIER
    }

    class DotFaultTreeEncoder : IFaultTreeCodec
    {
        private static Regex transition = new Regex(@"(?<from>\d*)[^-]*-\>[^\d]*(?<to>\d*).*");
        private static Regex rootPattern = new Regex(@"(?<id>\d*)[^\[]*\[shape=point.*");
        private static Regex gatePattern = new Regex("(?<id>\\d*)[^\\[]*\\[shape=box.*label=\"(?<typ>.*)\".*");
        private static Regex identifierPattern = new Regex("(?<id>\\d*)[^\\[]*\\[shape=circle.*label=\"(?<identifier>.*)\".*");

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
