// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BDDNode.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.BDD.BDDTree
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// The bdd node.
    /// </summary>
    public abstract class BDDNode
    {
        /// <summary>
        /// Gets or sets the high node.
        /// </summary>
        public BDDNode HighNode { get; set; }

        /// <summary>
        /// Gets or sets the low node.
        /// </summary>
        public BDDNode LowNode { get; set; }

        /// <summary>
        /// Gets or sets the variable.
        /// </summary>
        public int Variable { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BDDNode"/> class.
        /// </summary>
        protected BDDNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BDDNode"/> class.
        /// </summary>
        /// <param name="highNode">
        /// The high node.
        /// </param>
        /// <param name="lowNode">
        /// The low node.
        /// </param>
        protected BDDNode(BDDNode highNode, BDDNode lowNode)
        {
            this.HighNode = highNode;
            this.LowNode = lowNode;
        }

        /// <summary>
        /// The generated number.
        /// </summary>
        public static int GeneratedNumber;

        /// <summary>
        /// Recursive helper for FlatMap.
        /// </summary>
        /// <param name="visited">
        /// The already visited nodes.
        /// </param>
        private void FlatMap(ref List<BDDNode> visited) 
		{
			if (visited.Contains(this))
			{
				return;
			}

			visited.Add(this);

            if (this.GetType() != typeof(BDDVariableNode))
            {
                return;
            }

            this.HighNode.FlatMap(ref visited);
            this.LowNode.FlatMap(ref visited);
		}

        /// <summary>
        /// Flattens node structure of tree into list.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>List</cref>
        ///     </see>
        ///     .
        /// </returns>
        public List<BDDNode> FlatMap()
        {
            var flat = new List<BDDNode> { this };
            GeneratedNumber++;

            if (this.GetType() != typeof(BDDVariableNode))
            {
                return flat;
            }

            this.HighNode.FlatMap(ref flat);
            this.LowNode.FlatMap(ref flat);

		    return flat;
		}

        /// <summary>
        /// Flattens the tree structure into list.
        /// </summary>
        /// <param name="root">
        /// The root.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public static IEnumerable<BDDNode> Traverse(BDDNode root)
		{
			var stack = new Stack<BDDNode>();
			var visited = new HashSet<BDDNode>();
			stack.Push(root);
			while (stack.Count > 0)
			{
				BDDNode current = stack.Pop();

				if (visited.Contains(current))
				{
					continue;
				}

				visited.Add(current);
				yield return current;

			    if (current.GetType() != typeof(BDDVariableNode))
			    {
			        continue;
			    }

			    stack.Push(current.HighNode);
			    stack.Push(current.LowNode);
			}
		}

        /// <summary>
        /// Write BDD node into stream.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        public void WriteToFile(StreamWriter stream)
		{
			stream.WriteLine("digraph G {");
			stream.WriteLine("0 [shape=box, label=\"0\", style=filled, shape=box, height=0.3, width=0.3];");
			stream.WriteLine("1 [shape=box, label=\"1\", style=filled, shape=box, height=0.3, width=0.3];");

			GeneratedNumber = 0;

			List<BDDNode> flat = Traverse(this).ToList();
			flat = (from f in flat orderby f.Variable select f).Reverse().ToList();

			var fastAccess = new Dictionary<BDDNode, int>();
			for (int i = 0; i < flat.Count; i++)
			{
				fastAccess.Add(flat[i], i);
			}

			for (int i = 2; i < flat.Count; i++)
			{
				stream.WriteLine(i + " [label=\"" + flat[i].Variable + "\"];\n" + i + " -> " + fastAccess[flat[i].LowNode] + " [style=dotted];\n" + i + " -> " + fastAccess[flat[i].HighNode] + " [style=filled];");
			}

			stream.WriteLine("}");
		}
    }
}
