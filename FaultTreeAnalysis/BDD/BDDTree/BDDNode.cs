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

        protected BDDNode() { }

        protected BDDNode(BDDNode highNode, BDDNode lowNode)
        {
            this.HighNode = highNode;
            this.LowNode = lowNode;
        }

		public static int GeneratedNumber;

		private void FlatMap(ref List<BDDNode> visited) 
		{
			if (visited.Contains(this))
			{
				return;
			}

			visited.Add(this);

			if (GetType() == typeof(BDDVariableNode))
			{
				HighNode.FlatMap(ref visited);
				LowNode.FlatMap(ref visited);
			}
		}



		public List<BDDNode> FlatMap()
		{
			List<BDDNode> flat = new List<BDDNode>();
			flat.Add(this);
			GeneratedNumber++;
			if (GetType() == typeof(BDDVariableNode))
			{
				HighNode.FlatMap(ref flat);
				LowNode.FlatMap(ref flat);
			}

			return flat;
		}

		public static IEnumerable<BDDNode> Traverse(BDDNode root)
		{
			var stack = new Stack<BDDNode>();
			HashSet<BDDNode> visited = new HashSet<BDDNode>();
			stack.Push(root);
			while (stack.Count > 0)
			{
				var current = stack.Pop();

				if (visited.Contains(current))
				{
					continue;
				}
				visited.Add(current);
				yield return current;
				if (current.GetType() == typeof(BDDVariableNode))
				{
					stack.Push(current.HighNode);
					stack.Push(current.LowNode);
				}
				
			}
		}

		public void WriteToFile(StreamWriter test)
		{
			test.WriteLine("digraph G {");
			test.WriteLine("0 [shape=box, label=\"0\", style=filled, shape=box, height=0.3, width=0.3];");
			test.WriteLine("1 [shape=box, label=\"1\", style=filled, shape=box, height=0.3, width=0.3];");

			GeneratedNumber = 0;

			List<BDDNode> flat = Traverse(this).ToList();
			flat = (from f in flat orderby f.Variable select f).Reverse().ToList();

			Dictionary<BDDNode, int> fastAccess = new Dictionary<BDDNode, int>();
			for (int i = 0; i < flat.Count; i++)
			{
				fastAccess.Add(flat[i], i);
			}

			for (int i = 2; i < flat.Count; i++)
			{
				test.WriteLine(i + " [label=\"" + flat[i].Variable + "\"];\n" + i + " -> " + fastAccess[flat[i].LowNode] + " [style=dotted];\n" + i + " -> " + fastAccess[flat[i].HighNode] + " [style=filled];");
			}


			test.WriteLine("}");
		}
    }
}
