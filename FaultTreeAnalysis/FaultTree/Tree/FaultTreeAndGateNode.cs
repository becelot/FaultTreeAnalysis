using System.Collections.Generic;
using System.Linq;
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
            var l = Childs.Select(c => c.Reduce(tr)).ToList();

            return tr.Transform(this, l);
        }
    }
}
