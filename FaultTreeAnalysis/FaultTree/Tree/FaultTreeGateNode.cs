using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    public class FaultTreeGateNode : FaultTreeNode
    {
        public enum FaultTreeGateOperator
        {
            FAULT_TREE_OPERATOR_AND,
            FAULT_TREE_OPERATOR_OR
        }

        private static FaultTreeGateOperator operatorFromString(String op)
        {
            return op.Equals("&") ? FaultTreeGateOperator.FAULT_TREE_OPERATOR_AND : FaultTreeGateOperator.FAULT_TREE_OPERATOR_OR;
        }

        [DataMember()]
        public FaultTreeGateOperator Operator { get; set; }

        public FaultTreeGateNode(int ID) : base(ID) { }

        public FaultTreeGateNode() : base() { }

    }
}
