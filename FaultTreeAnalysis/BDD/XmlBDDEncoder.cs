// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XmlBDDEncoder.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.BDD
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Xml;

    using FaultTreeAnalysis.BDD.BDDTree;

    /// <summary>
    /// The xml bdd encoder.
    /// </summary>
    internal class XmlBDDEncoder : IBDDCodec
	{
        /// <summary>
        /// Reads a Binary Decision Diagram from a stream.
        /// </summary>
        /// <param name="fileName">
        /// The file sttream.
        /// </param>
        /// <returns>
        /// The <see cref="BinaryDecisionDiagram"/>.
        /// </returns>
        public override BinaryDecisionDiagram Read(FileStream fileName)
		{
			var listOfNodes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
							   from lType in lAssembly.GetTypes()
							   where typeof(BDDNode).IsAssignableFrom(lType)
							   select lType).ToArray();
		    var setting = new DataContractSerializerSettings
		    {
		        PreserveObjectReferences = true,
		        KnownTypes = listOfNodes
		    };
		    var serializer = new DataContractSerializer(typeof(BinaryDecisionDiagram), setting);
			return (BinaryDecisionDiagram)serializer.ReadObject(fileName);
		}

        /// <summary>
        /// Writes a given BDD to a stream.
        /// </summary>
        /// <param name="bdd">
        /// The bdd.
        /// </param>
        /// <param name="stream">
        /// The stream.
        /// </param>
        public override void Write(BinaryDecisionDiagram bdd, FileStream stream)
		{
			var listOfNodes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
							   from lType in lAssembly.GetTypes()
							   where typeof(BDDNode).IsAssignableFrom(lType)
							   select lType).ToArray();
			var setting = new DataContractSerializerSettings { PreserveObjectReferences = true, KnownTypes = listOfNodes };

			var xmlSettings = new XmlWriterSettings { Indent = true };

			var serializer = new DataContractSerializer(typeof(BinaryDecisionDiagram), setting);
			using (XmlWriter w = XmlWriter.Create(stream, xmlSettings))
			{
				serializer.WriteObject(XmlWriter.Create(w, xmlSettings), bdd);
			}
		}

        /// <summary>
        /// The format token.
        /// </summary>
        /// <returns>
        /// The <see cref="BDDTreeFormat"/>.
        /// </returns>
        public override BDDTreeFormat GetFormatToken()
		{
			return BDDTreeFormat.BDD_TREE_XML;
		}
	}
}
