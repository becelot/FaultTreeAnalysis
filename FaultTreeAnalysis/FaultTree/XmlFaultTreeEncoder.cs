// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlFaultTreeEncoder.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.FaultTree
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml;

    using FaultTreeAnalysis.FaultTree.Tree;

    /// <summary>
    /// The xml fault tree encoder.
    /// </summary>
    internal class XmlFaultTreeEncoder : IFaultTreeCodec
    {
        /// <summary>
        /// Read Fault Tree from XML formatted stream.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <returns>
        /// The <see cref="FaultTree"/>.
        /// </returns>
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

        /// <summary>
        /// Writes Fault Tree to stream
        /// </summary>
        /// <param name="ft">
        /// The Fault Tree.
        /// </param>
        /// <param name="stream">
        /// The stream.
        /// </param>
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

        /// <summary>
        /// The format token.
        /// </summary>
        /// <returns>
        /// The <see cref="FaultTreeFormat"/>.
        /// </returns>
        public override FaultTreeFormat GetFormatToken()
        {
            return FaultTreeFormat.FAULT_TREE_XML;
        }
    }
}
