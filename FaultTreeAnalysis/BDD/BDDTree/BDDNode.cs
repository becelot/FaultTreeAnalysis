using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.BDD.BDDTree
{
    public abstract class BDDNode
    {
        public BDDNode HighNode { get; set; }
        public BDDNode LowNode { get; set; }

        public BDDNode() { }
        public BDDNode(BDDNode HighNode, BDDNode LowNode)
        {
            this.HighNode = HighNode;
            this.LowNode = LowNode;
        } 
    }
}
