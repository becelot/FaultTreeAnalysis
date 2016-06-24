using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;

namespace FaultTreeAnalysis.FaultTree
{
    class XmlFaultTreeEncoder : IFaultTreeCodec
    {

        public override FaultTree read(FileStream stream)
        {
            var listOfNodes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                               from lType in lAssembly.GetTypes()
                               where typeof(FaultTreeNode).IsAssignableFrom(lType)
                               select lType).ToArray();
            var setting = new DataContractSerializerSettings();
            setting.PreserveObjectReferences = true;
            setting.KnownTypes = listOfNodes;
            var serializer = new DataContractSerializer(typeof(FaultTree), setting);
            return (FaultTree)serializer.ReadObject(stream);
        }

        public override void write(FaultTree ft, FileStream stream)
        {
            var listOfNodes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                               from lType in lAssembly.GetTypes()
                               where typeof(FaultTreeNode).IsAssignableFrom(lType)
                               select lType).ToArray();
            var setting = new DataContractSerializerSettings { PreserveObjectReferences = true, KnownTypes = listOfNodes };

            XmlWriterSettings xmlSettings = new XmlWriterSettings { Indent = true };

            var serializer = new DataContractSerializer(typeof(FaultTree), setting);
            using (var w = XmlWriter.Create(stream, xmlSettings))
            {
                serializer.WriteObject(XmlWriter.Create(w, xmlSettings), ft);
            }
            
        }

        public override FaultTreeFormat getFormatToken()
        {
            return FaultTreeFormat.FAULT_TREE_XML;
        }
    }
}
