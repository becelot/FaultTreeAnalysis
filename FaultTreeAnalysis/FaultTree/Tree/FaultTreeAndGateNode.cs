using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    [DataContract(Name = "FaultTreeAndGateNode")]
    public class FaultTreeAndGateNode : FaultTreeGateNode
    {
        public FaultTreeAndGateNode(int ID) : base(ID) { }
        public FaultTreeAndGateNode() : base() { }

        public FaultTreeAndGateNode(int ID, List<FaultTreeNode> childs) : base(ID, childs) { }

        public override FaultTreeNode reduce(TreeTransformer tr)
        {
            List<FaultTreeNode> l = new List<FaultTreeNode>();

            foreach (FaultTreeNode c in Childs)
            {
                l.Add(c.reduce(tr));
            }

            return tr.transform(this, l);
        }
    }
}
