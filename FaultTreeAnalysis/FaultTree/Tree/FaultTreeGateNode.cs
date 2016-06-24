using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    [DataContract(Name = "FaultTreeGateNode")]
    public abstract class FaultTreeGateNode : FaultTreeNode
    {
        protected FaultTreeGateNode(int id) : base(id) { }

        protected FaultTreeGateNode()
        { }

        protected FaultTreeGateNode(int id, List<FaultTreeNode> childs) : base(id, childs) { }

    }
}
