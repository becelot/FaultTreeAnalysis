using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    public class FaultTreeGateNode : FaultTreeNode
    {
        public enum FaultTreeGateOperator
        {
            FAULT_TREE_OPERATOR_AND,
            FAULT_TREE_OPERATOR_OR,
            FAULT_TREE_OPERATOR_EQUAL
        }

        private static FaultTreeGateOperator operatorFromString(String op)
        {
            return op.Equals("&") ? FaultTreeGateOperator.FAULT_TREE_OPERATOR_AND : FaultTreeGateOperator.FAULT_TREE_OPERATOR_OR;
        }


        public FaultTreeGateOperator Operator { get; set; }

        public FaultTreeGateNode()
        {

        }

        public FaultTreeGateNode(int id, FaultTreeGateOperator op)
        {
            this.ID = id;
            this.Operator = op;
        }

        public FaultTreeGateNode(int id, string op) : this(id, FaultTreeGateNode.operatorFromString(op))
        {
            
        }
    }
}
