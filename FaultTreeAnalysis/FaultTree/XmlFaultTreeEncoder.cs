using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree
{
    internal class XmlFaultTreeEncoder : IFaultTreeCodec
    {

        public override FaultTree Read(FileStream stream)
        {
            var listOfNodes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                               from lType in lAssembly.GetTypes()
                               where typeof(FaultTreeNode).IsAssignableFrom(lType)
                               select lType).ToArray();
            var setting = new DataContractSerializerSettings
            {
                PreserveObjectReferences = true,
                KnownTypes = listOfNodes
            };
            var serializer = new DataContractSerializer(typeof(FaultTree), setting);
            return (FaultTree)serializer.ReadObject(stream);
        }

        public override void Write(FaultTree ft, FileStream stream)
        {
            var listOfNodes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
                               from lType in lAssembly.GetTypes()
                               where typeof(FaultTreeNode).IsAssignableFrom(lType)
                               select lType).ToArray();
            var setting = new DataContractSerializerSettings { PreserveObjectReferences = true, KnownTypes = listOfNodes };

            var xmlSettings = new XmlWriterSettings { Indent = true };

            var serializer = new DataContractSerializer(typeof(FaultTree), setting);
            using (XmlWriter w = XmlWriter.Create(stream, xmlSettings))
            {
                serializer.WriteObject(XmlWriter.Create(w, xmlSettings), ft);
            }
            
        }

        public override FaultTreeFormat GetFormatToken()
        {
            return FaultTreeFormat.FAULT_TREE_XML;
        }
    }
}
