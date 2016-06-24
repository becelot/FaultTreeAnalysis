using System;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    public class FaultTreeNodeFactory
    {
        private static FaultTreeNodeFactory _instance;
        
        private FaultTreeNodeFactory() { }

        public enum FaultTreeGateOperator
        {
            FAULT_TREE_OPERATOR_AND,
            FAULT_TREE_OPERATOR_OR
        }

        private static FaultTreeGateOperator OperatorFromString(String op)
        {
            return op.Equals("&") ? FaultTreeGateOperator.FAULT_TREE_OPERATOR_AND : FaultTreeGateOperator.FAULT_TREE_OPERATOR_OR;
        }

        public static FaultTreeNodeFactory GetInstance()
        {
            return _instance ?? (_instance = new FaultTreeNodeFactory());
        }

        public FaultTreeGateNode CreateGateNode(int id, FaultTreeGateOperator op)
        {
            switch (op)
            {
                case FaultTreeGateOperator.FAULT_TREE_OPERATOR_AND:
                    return new FaultTreeAndGateNode(id);
                default:
                    return new FaultTreeOrGateNode(id);
            }
        }

        public FaultTreeGateNode CreateGateNode(int id, String operation)
        {
            return CreateGateNode(id, OperatorFromString(operation));
        }
    }
}
