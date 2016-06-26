using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using FaultTreeAnalysis.FaultTree.MarkovChain;
using FaultTreeAnalysis.FaultTree.Transformer;
using FaultTreeAnalysis.FaultTree.Tree;
using MathNet.Numerics.LinearAlgebra;

namespace FaultTreeAnalysis.FaultTree
{
    [DataContract(Name = "FaultTree")]
    public class FaultTree
    {
        [DataMember]
        public FaultTreeNode Root { get; set; }

		public MarkovChain<FaultTreeTerminalNode> MarkovChain { get; set; }

	    [DataMember]
	    private IEnumerable<Tuple<FaultTreeTerminalNode, double, FaultTreeTerminalNode>> MarkovArray
	    {
		    get { return MarkovChain.GetAllEdges();}
		    set
		    {
			    int row = value.Select(v => v.Item1).Union(value.Select(v => v.Item3)).GroupBy(v => v).Count();
			    //MarkovChain = new MarkovChain<FaultTreeTerminalNode>(Matrix<double>.Build.DenseOfIndexed(row, row, value));
				MarkovChain = new MarkovChain<FaultTreeTerminalNode>(row);
			    foreach (var trans in value)
			    {
				    MarkovChain[trans.Item1, trans.Item3] = trans.Item2;
			    }
		    }
	    }

	    public FaultTree()
        {

        }

        public FaultTree(FaultTreeNode root)
        {
            Root = root;
			MarkovChain = new MarkovChain<FaultTreeTerminalNode>(1);
        }

	    public FaultTree(FaultTreeNode root, MarkovChain<FaultTreeTerminalNode> markovChain)
	    {
		    MarkovChain = markovChain;
		    Root = root;
	    }

        public T Reduce<T>(FaultTreeTransformer<T> tr)
        {
            return Root.Reduce(tr);
        }

        public FaultTree TreeMap(FaultTreeTransformer<FaultTreeNode> tr)
        {
            return new FaultTree( Root.Reduce(tr) );
        }

        public FaultTree DeepCopy()
        {
            return TreeMap(new DeepCopyTransformer());
        }

        public FaultTree Replace(int label, bool value)
        {
            return TreeMap(new ReplaceTransformer(label, value));
        }

        public FaultTree Simplify()
        {
            return TreeMap(new SimplifyTransformer());
        }

		public IEnumerable<FaultTreeNode> Traverse()
		{
			Stack<FaultTreeNode> stack = new Stack<FaultTreeNode>();

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

			    var node = current as FaultTreeGateNode;
			    if (node == null) continue;

			    foreach (var n  in node.Childs)
			    {
			        stack.Push(n);
			    }
			}
		}
	}
}
