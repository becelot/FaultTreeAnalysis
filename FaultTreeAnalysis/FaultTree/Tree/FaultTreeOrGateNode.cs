using FaultTreeAnalysis.FaultTree.Transformer;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    [DataContract(Name = "FaultTreeOrGateNode")]
    public class FaultTreeOrGateNode : FaultTreeGateNode
    {
        public FaultTreeOrGateNode(int ID) : base(ID) { }
        public FaultTreeOrGateNode()  { }

        public FaultTreeOrGateNode(int ID, List<FaultTreeNode> childs) : base(ID, childs) { }

        public override T reduce<T>(FaultTreeTransformer<T> tr)
        {
            List<T> l = new List<T>();

            foreach (FaultTreeNode c in Childs)
            {
                l.Add(c.reduce(tr));
            }

            return tr.transform(this, l);
        }
    }
}
