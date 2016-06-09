using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.BDD.BDDTree
{
    public class BDDVariableNode : BDDNode
    {
        public int Variable { get; set; }

        public BDDVariableNode() : base() { }
        public BDDVariableNode(int Variable, BDDNode HighNode, BDDNode LowNode) : base(HighNode, LowNode)
        {
            this.Variable = Variable;
        }
    }
}
