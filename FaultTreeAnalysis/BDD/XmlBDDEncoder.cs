﻿using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using FaultTreeAnalysis.BDD.BDDTree;

namespace FaultTreeAnalysis.BDD
{
	class XmlBDDEncoder : IBDDCodec
	{
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

		public override void Write(BinaryDecisionDiagram bdd, FileStream stream)
		{
			var listOfNodes = (from lAssembly in AppDomain.CurrentDomain.GetAssemblies()
							   from lType in lAssembly.GetTypes()
							   where typeof(BDDNode).IsAssignableFrom(lType)
							   select lType).ToArray();
			var setting = new DataContractSerializerSettings { PreserveObjectReferences = true, KnownTypes = listOfNodes };

			XmlWriterSettings xmlSettings = new XmlWriterSettings { Indent = true };

			var serializer = new DataContractSerializer(typeof(BinaryDecisionDiagram), setting);
			using (var w = XmlWriter.Create(stream, xmlSettings))
			{
				serializer.WriteObject(XmlWriter.Create(w, xmlSettings), bdd);
			}
		}

		public override BDDTreeFormat GetFormatToken()
		{
			return BDDTreeFormat.BDD_TREE_XML;
		}
	}
}
