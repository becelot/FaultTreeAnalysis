namespace FaultTreeAnalysis.FaultTree
{
    public class FaultTreeEncoderFactory
    {
        public static IFaultTreeCodec createFaultTreeCodec(string fileName)
        {
            if (fileName.EndsWith(".xml"))
            {
                return new XmlFaultTreeEncoder();
            } else if (fileName.EndsWith(".dot"))
            {
                return new DotFaultTreeEncoder();
            }

            throw new FaultTreeFormatException("The given file was not recognized as a valid format!"); 
        }

        public static IFaultTreeCodec createFaultTreeCodec(FaultTreeFormat format)
        {
            switch (format)
            {
                case FaultTreeFormat.FAULT_TREE_DOT: return new DotFaultTreeEncoder();
                case FaultTreeFormat.FAULT_TREE_XML: return new XmlFaultTreeEncoder();
                default: return null;
            }
        }
    }
}
