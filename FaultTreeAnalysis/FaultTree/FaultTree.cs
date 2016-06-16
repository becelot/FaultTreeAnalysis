using FaultTreeAnalysis.FaultTree.Transformer;
using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.Runtime.Serialization;

namespace FaultTreeAnalysis.FaultTree
{
    [DataContract(Name = "FaultTree")]
    public class FaultTree
    {
        [DataMember()]
        public FaultTreeNode Root { get; set; }

        public FaultTree()
        {

        }

        public FaultTree(FaultTreeNode root)
        {
            this.Root = root;
        }

        public T reduce<T>(FaultTreeTransformer<T> tr)
        {
            return Root.reduce<T>(tr);
        }

        public FaultTree treeMap<T>(FaultTreeTransformer<FaultTreeNode> tr)
        {
            return new FaultTree( Root.reduce(tr) );
        }

        public FaultTree deepCopy()
        {
            return treeMap<FaultTreeNode>(new DeepCopyTransformer());
        }

        public FaultTree replace(int label, Boolean value)
        {
            return treeMap<FaultTreeNode>(new ReplaceTransformer(label, value));
        }

        public FaultTree simplify()
        {
            return treeMap<FaultTreeNode>(new SimplifyTransformer());
        }
    }
}
