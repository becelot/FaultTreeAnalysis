using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    [DataContract(Name = "FaultTreeOrGateNode")]
    public class FaultTreeOrGateNode : FaultTreeGateNode
    {
        public FaultTreeOrGateNode(int ID) : base(ID) { }
        public FaultTreeOrGateNode()  { }

        public FaultTreeOrGateNode(int ID, List<FaultTreeNode> childs) : base(ID, childs) { }

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
