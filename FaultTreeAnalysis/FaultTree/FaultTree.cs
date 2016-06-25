using System.Collections.Generic;
using System.Runtime.Serialization;
using FaultTreeAnalysis.FaultTree.Transformer;
using FaultTreeAnalysis.FaultTree.Tree;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;

namespace FaultTreeAnalysis.FaultTree
{
    [DataContract(Name = "FaultTree")]
    public class FaultTree
    {
        [DataMember]
        public FaultTreeNode Root { get; set; }

		[DataMember]
		public Matrix<double> GeneratorMatrix { get; set; }

        public FaultTree()
        {

        }

        public FaultTree(FaultTreeNode root)
        {
            Root = root;
			GeneratorMatrix = Matrix<double>.Build.Dense(1, 1);
        }

	    public FaultTree(FaultTreeNode root, Matrix<double> generatorMatrix)
	    {
		    this.GeneratorMatrix = generatorMatrix;
		    this.Root = root;
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
