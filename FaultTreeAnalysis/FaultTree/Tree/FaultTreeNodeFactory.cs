using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    public class FaultTreeNodeFactory
    {
        private static FaultTreeNodeFactory _instance = null;
        
        private FaultTreeNodeFactory() { }

        public enum FaultTreeGateOperator
        {
            FAULT_TREE_OPERATOR_AND,
            FAULT_TREE_OPERATOR_OR
        }

        private static FaultTreeGateOperator operatorFromString(String op)
        {
            return op.Equals("&") ? FaultTreeGateOperator.FAULT_TREE_OPERATOR_AND : FaultTreeGateOperator.FAULT_TREE_OPERATOR_OR;
        }

        public static FaultTreeNodeFactory getInstance()
        {
            if (_instance == null)
            {
                _instance = new FaultTreeNodeFactory();
            }
            return _instance;
        }

        public FaultTreeGateNode createGateNode(int ID, FaultTreeGateOperator op)
        {
            switch (op)
            {
                case FaultTreeGateOperator.FAULT_TREE_OPERATOR_AND:
                    return new FaultTreeAndGateNode(ID);
                default:
                    return new FaultTreeOrGateNode(ID);
            }
        }

        public FaultTreeGateNode createGateNode(int ID, string operation)
        {
            return this.createGateNode(ID, operatorFromString(operation));
        }
    }
}
