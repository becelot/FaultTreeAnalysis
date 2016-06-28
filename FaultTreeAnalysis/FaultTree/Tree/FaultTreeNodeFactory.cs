// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultTreeNodeFactory.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FaultTreeAnalysis.FaultTree.Tree
{
    /// <summary>
    /// The fault tree node factory.
    /// </summary>
    public class FaultTreeNodeFactory
    {
        /// <summary>
        /// instance of singleton implementation.
        /// </summary>
        private static FaultTreeNodeFactory instance;

        /// <summary>
        /// Prevents a default instance of the <see cref="FaultTreeNodeFactory"/> class from being created.
        /// </summary>
        private FaultTreeNodeFactory()
        {
        }

        /// <summary>
        /// The fault tree gate operator.
        /// </summary>
        public enum FaultTreeGateOperator
        {
            /// <summary>
            /// And Gate
            /// </summary>
            FAULT_TREE_OPERATOR_AND,

            /// <summary>
            /// Or Gate
            /// </summary>
            FAULT_TREE_OPERATOR_OR
        }

        /// <summary>
        /// Converts string to operator value
        /// </summary>
        /// <param name="op">
        /// The op.
        /// </param>
        /// <returns>
        /// The <see cref="FaultTreeGateOperator"/>.
        /// </returns>
        private static FaultTreeGateOperator OperatorFromString(string op)
        {
            return op.Equals("&") ? FaultTreeGateOperator.FAULT_TREE_OPERATOR_AND : FaultTreeGateOperator.FAULT_TREE_OPERATOR_OR;
        }

        /// <summary>
        /// The singleton instance.
        /// </summary>
        /// <returns>
        /// The <see cref="FaultTreeNodeFactory"/>.
        /// </returns>
        public static FaultTreeNodeFactory GetInstance()
        {
            return instance ?? (instance = new FaultTreeNodeFactory());
        }

        /// <summary>
        /// Creates a new gate node.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="op">
        /// The operation.
        /// </param>
        /// <returns>
        /// The <see cref="FaultTreeGateNode"/>.
        /// </returns>
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

        /// <summary>
        /// Creates a new gate node.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="operation">
        /// The operation.
        /// </param>
        /// <returns>
        /// The <see cref="FaultTreeGateNode"/>.
        /// </returns>
        public FaultTreeGateNode CreateGateNode(int id, string operation)
        {
            return this.CreateGateNode(id, OperatorFromString(operation));
        }
    }
}
