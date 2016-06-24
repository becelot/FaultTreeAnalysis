using System.Collections.Generic;
using System.Runtime.Serialization;
using FaultTreeAnalysis.FaultTree.Transformer;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    [DataContract(Name = "FaultTreeAndGateNode")]
    public class FaultTreeAndGateNode : FaultTreeGateNode
    {
        public FaultTreeAndGateNode(int id) : base(id) { }
        public FaultTreeAndGateNode()
        { }

        public FaultTreeAndGateNode(int id, List<FaultTreeNode> childs) : base(id, childs) { }

        public override T Reduce<T>(FaultTreeTransformer<T> tr)
        {
            List<T> l = new List<T>();

            foreach (FaultTreeNode c in Childs)
            {
                l.Add(c.Reduce(tr));
            }

            return tr.Transform(this, l);
        }
    }
}
