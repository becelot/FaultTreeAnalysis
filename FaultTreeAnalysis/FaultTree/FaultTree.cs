using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using FaultTreeAnalysis.FaultTree.Transformer;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree
{
    [DataContract(Name = "FaultTree")]
    public class FaultTree
    {
        [DataMember]
        public FaultTreeNode Root { get; set; }

        public FaultTree()
        {

        }

        public FaultTree(FaultTreeNode root)
        {
            Root = root;
        }

        public T Reduce<T>(FaultTreeTransformer<T> tr)
        {
            return Root.Reduce(tr);
        }

        public FaultTree TreeMap<T>(FaultTreeTransformer<FaultTreeNode> tr)
        {
            return new FaultTree( Root.Reduce(tr) );
        }

        public FaultTree DeepCopy()
        {
            return TreeMap<FaultTreeNode>(new DeepCopyTransformer());
        }

        public FaultTree Replace(int label, Boolean value)
        {
            return TreeMap<FaultTreeNode>(new ReplaceTransformer(label, value));
        }

        public FaultTree Simplify()
        {
            return TreeMap<FaultTreeNode>(new SimplifyTransformer());
        }

		public IEnumerable<FaultTreeNode> Traverse()
		{
			var stack = new Stack<FaultTreeNode>();

			HashSet<FaultTreeNode> visited = new HashSet<FaultTreeNode>();
			stack.Push(Root);

			while (stack.Count > 0)
			{
				var current = stack.Pop();

				if (visited.Contains(current))
				{
					continue;
				}
				visited.Add(current);
				yield return current;
				if (current is FaultTreeGateNode)
				{
					foreach (FaultTreeNode n  in ((FaultTreeGateNode)current).Childs)
					{
						stack.Push(n);
					}
				}

			}
		}
	}
}
