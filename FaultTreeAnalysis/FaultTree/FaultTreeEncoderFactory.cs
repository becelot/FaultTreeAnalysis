using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree
{
    public class FaultTreeEncoderFactory
    {
        public static IFaultTreeCodec createFaultTreeCodec(string fileName)
        {
            if (fileName.EndsWith(".xml"))
            {
                return new XmlFaultTreeCodec();
            } else if (fileName.EndsWith(".dot"))
            {
                return new DotFaultTreeEncoder();
            }

            throw new FaultTreeFormatException("The given file was not recognized as a valid format!"); 
        }
    }
}
