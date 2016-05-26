using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree
{
    class XmlFaultTreeEncoder : IFaultTreeCodec
    {

        public override FaultTree read(StreamReader stream)
        {
            XmlSerializer xml = new XmlSerializer(typeof(FaultTree));
            return (FaultTree)xml.Deserialize(stream);
        }

        public override void write(FaultTree ft, StreamWriter stream)
        {
            var listOfNodes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                               from lType in lAssembly.GetTypes()
                               where typeof(FaultTreeNode).IsAssignableFrom(lType)
                               select lType).ToArray();

            XmlSerializer xml = new XmlSerializer(typeof(FaultTree), listOfNodes);
            xml.Serialize(stream, ft);
        }

        public override FaultTreeFormat getFormatToken()
        {
            return FaultTreeFormat.FAULT_TREE_XML;
        }
    }
}
