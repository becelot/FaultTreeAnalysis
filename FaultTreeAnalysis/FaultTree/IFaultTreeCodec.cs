using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree
{
    public enum FaultTreeFormat
    {
        FAULT_TREE_UNKNOWN = 0,
        FAULT_TREE_DOT,
        FAULT_TREE_XML
    };

    public abstract class IFaultTreeCodec
    {

        public abstract void write(FaultTree ft);

        public abstract FaultTree read(String fileName);

        public virtual FaultTreeFormat getFormatToken() { return FaultTreeFormat.FAULT_TREE_UNKNOWN; }
    }
}
