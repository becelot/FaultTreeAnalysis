using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree
{
    class XmlFaultTreeCodec : IFaultTreeCodec
    {
        public override FaultTree read(string fileName)
        {
            throw new NotImplementedException();
        }

        public override void write(FaultTree ft)
        {
            throw new NotImplementedException();
        }

        public override FaultTreeFormat getFormatToken()
        {
            return FaultTreeFormat.FAULT_TREE_XML;
        }
    }
}
