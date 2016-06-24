using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    [DataContract(Name = "FaultTreeGateNode")]
    public abstract class FaultTreeGateNode : FaultTreeNode
    {
        protected FaultTreeGateNode(int ID) : base(ID) { }

        protected FaultTreeGateNode()
        { }

        protected FaultTreeGateNode(int ID, List<FaultTreeNode> childs) : base(ID, childs) { }

    }
}
