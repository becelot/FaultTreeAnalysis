﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FaultTreeAnalysis.BDD.BDDTree;

namespace FaultTreeAnalysis.BDD
{
    internal class DotBDDEncoder : IBDDCodec
	{
		public override BinaryDecisionDiagram Read(FileStream fileName)
		{
			throw new NotImplementedException();
		}

		public override void Write(BinaryDecisionDiagram bdd, FileStream stream)
		{
			using (var sw = new StreamWriter(stream, Encoding.UTF8, 65536))
			{
				sw.WriteLine("digraph G {");
				sw.WriteLine("0 [shape=box, label=\"0\", style=filled, shape=box, height=0.3, width=0.3];");
				sw.WriteLine("1 [shape=box, label=\"1\", style=filled, shape=box, height=0.3, width=0.3];");

				List<BDDNode> flat = BDDNode.Traverse(bdd.Root).ToList();
				flat = (from f in flat orderby f.Variable select f).Reverse().ToList();

				var fastAccess = new Dictionary<BDDNode, int>();
				for (int i = 0; i < flat.Count; i++)
				{
					fastAccess.Add(flat[i], i);
				}

				for (int i = 2; i < flat.Count; i++)
				{
					sw.WriteLine(i + " [label=\"" + flat[i].Variable + "\"];\n" + i + " -> " + fastAccess[flat[i].LowNode] + " [style=dotted];\n" + i + " -> " + fastAccess[flat[i].HighNode] + " [style=filled];");
				}


				sw.WriteLine("}");
			}

		}

		public override BDDTreeFormat GetFormatToken()
		{
			return BDDTreeFormat.BDD_TREE_DOT;
		}
	}
}
