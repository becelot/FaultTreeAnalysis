using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FaultTreeAnalysis.BDD.BDDTree
{
    public abstract class BDDNode
    {
        public BDDNode HighNode { get; set; }
        public BDDNode LowNode { get; set; }

		public int Variable { get; set; }

        public BDDNode() { }
        public BDDNode(BDDNode HighNode, BDDNode LowNode)
        {
            this.HighNode = HighNode;
            this.LowNode = LowNode;
        }

		private void flatMap(HashSet<BDDNode> visited) 
		{
			if (visited.Contains(this))
			{
				return;
			}

			visited.Add(this);

			if (this.GetType() == typeof(BDDVariableNode))
			{
				HighNode.flatMap(visited);
				LowNode.flatMap(visited);
			}
		}

		public HashSet<BDDNode> flatMap()
		{
			HashSet<BDDNode> flat = new HashSet<BDDNode>();
			flat.Add(this);

			if (this.GetType() == typeof(BDDVariableNode))
			{
				HighNode.flatMap(flat);
				LowNode.flatMap(flat);
			}

			return flat;
		}

		public override String ToString()
		{
			StringWriter sw = new StringWriter();
			sw.WriteLine("digraph G {");
			sw.WriteLine("0 [shape=box, label=\"0\", style=filled, shape=box, height=0.3, width=0.3];");
			sw.WriteLine("1 [shape=box, label=\"1\", style=filled, shape=box, height=0.3, width=0.3];");

			List<BDDNode> flat = new List<BDDNode>(this.flatMap());
			flat =	(from f in flat orderby f.Variable select f).ToList();

			for (int i = 2; i < flat.Count; i++)
			{
				sw.WriteLine(i + " [label=\"" + flat[i].Variable + "\"];");
				sw.WriteLine(i + " -> " + flat.IndexOf(flat[i].LowNode) + " [style=dotted];");
				sw.WriteLine(i + " -> " + flat.IndexOf(flat[i].HighNode) + " [style=filled];");
			}


			sw.WriteLine("}");

			return sw.ToString();
		}
    }
}
