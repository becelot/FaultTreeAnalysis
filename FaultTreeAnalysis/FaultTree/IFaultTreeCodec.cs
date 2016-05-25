using System;
using System.Collections.Generic;
using System.IO;
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

        public abstract void write(FaultTree ft, StreamWriter stream);

        public abstract FaultTree read(StreamReader fileName);

        public virtual FaultTreeFormat getFormatToken() { return FaultTreeFormat.FAULT_TREE_UNKNOWN; }

        public void write(FaultTree ft, String fileName)
        {
            using (StreamWriter stream = new StreamWriter(fileName))
            {
                this.write(ft, stream);
            }
        }

        public FaultTree read(String fileName)
        {
            FaultTree res;

            //Create stream from file contents
            using (StreamReader stream = new StreamReader(fileName))
            {
                res = this.read(stream);
            }

            return res;
        }
    }
}
