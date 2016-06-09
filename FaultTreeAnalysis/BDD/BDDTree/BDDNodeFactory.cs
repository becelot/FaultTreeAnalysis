using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.BDD.BDDTree
{
    public class BDDNodeFactory
    {
        public static BDDNode createNode(int value, BDDNode HighNode, BDDNode LowNode)
        {
            return new BDDVariableNode(value, HighNode, LowNode);
        }

        public static BDDNode createNode(Boolean value)
        {
            return new BDDTerminalNode(value);
        }
    }
}
