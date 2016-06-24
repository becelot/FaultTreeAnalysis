using System;
using System.IO;

namespace FaultTreeAnalysis.FaultTree
{
    public enum FaultTreeFormat
    {
        FAULT_TREE_UNKNOWN = 0,
        FAULT_TREE_DOT,
        FAULT_TREE_XML
    }

    public abstract class IFaultTreeCodec
    {

        public abstract void write(FaultTree ft, FileStream stream);

        public abstract FaultTree read(FileStream fileName);

        public virtual FaultTreeFormat getFormatToken() { return FaultTreeFormat.FAULT_TREE_UNKNOWN; }

        public void write(FaultTree ft, String fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                write(ft, stream);
            }
        }

        public FaultTree read(String fileName)
        {
            FaultTree res;

            //Create stream from file contents
            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                res = read(stream);
            }

            return res;
        }
    }
}
