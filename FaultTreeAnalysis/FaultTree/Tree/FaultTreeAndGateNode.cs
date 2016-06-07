using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    public class FaultTreeAndGateNode : FaultTreeGateNode
    {
        public FaultTreeAndGateNode(int ID) : base(ID) { }
        public FaultTreeOrGateNode() : base() { }
    }
}
