using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    [DataContract(Name = "FaultTreeGateNode")]
    public abstract class FaultTreeGateNode : FaultTreeNode
    {
        public FaultTreeGateNode(int ID) : base(ID) { }

        public FaultTreeGateNode() : base() { }

        public FaultTreeGateNode(int ID, List<FaultTreeNode> childs) : base(ID, childs) { }

    }
}
