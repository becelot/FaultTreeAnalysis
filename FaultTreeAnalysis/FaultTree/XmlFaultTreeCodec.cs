using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FaultTreeAnalysis.FaultTree
{
    class XmlFaultTreeCodec : IFaultTreeCodec
    {
        public override FaultTree read(StreamReader stream)
        {
            XmlSerializer xml = new XmlSerializer(typeof(FaultTree));
            return (FaultTree)xml.Deserialize(stream);
        }

        public override void write(FaultTree ft, StreamWriter stream)
        {
            XmlSerializer xml = new XmlSerializer(typeof(FaultTree));
            xml.Serialize(stream, ft);
        }

        public override FaultTreeFormat getFormatToken()
        {
            return FaultTreeFormat.FAULT_TREE_XML;
        }
    }
}
