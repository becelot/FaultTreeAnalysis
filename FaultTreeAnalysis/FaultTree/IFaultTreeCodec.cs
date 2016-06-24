using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FaultTreeAnalysis.FaultTree
{
    public enum FaultTreeFormat
    {
        FAULT_TREE_UNKNOWN = 0,
        FAULT_TREE_DOT,
        FAULT_TREE_XML
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class IFaultTreeCodec
    {

        public abstract void Write(FaultTree ft, FileStream stream);

        public abstract FaultTree Read(FileStream fileName);

        public virtual FaultTreeFormat GetFormatToken() { return FaultTreeFormat.FAULT_TREE_UNKNOWN; }

        public void Write(FaultTree ft, string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                Write(ft, stream);
            }
        }

        public FaultTree Read(string fileName)
        {
            FaultTree res;

            //Create stream from file contents
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                res = Read(stream);
            }

            return res;
        }
    }
}
