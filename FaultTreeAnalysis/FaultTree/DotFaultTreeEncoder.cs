using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree
{
    class DotFaultTreeEncoder : IFaultTreeCodec
    {
        public override FaultTree read(StreamReader stream)
        {
            throw new NotImplementedException();
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
