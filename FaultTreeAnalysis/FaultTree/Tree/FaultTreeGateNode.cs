using System.Collections.Generic;
using System.Runtime.Serialization;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    [DataContract(Name = "FaultTreeGateNode")]
    public abstract class FaultTreeGateNode : FaultTreeNode
    {
        public FaultTreeGateNode(int ID) : base(ID) { }

        public FaultTreeGateNode()
        { }

        public FaultTreeGateNode(int ID, List<FaultTreeNode> childs) : base(ID, childs) { }

    }
}
