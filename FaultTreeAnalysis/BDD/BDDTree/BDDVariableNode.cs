// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BDDVariableNode.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.BDD.BDDTree
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The bdd variable node.
    /// </summary>
    [DataContract(Name = "BDDVariableNode")]
    public class BDDVariableNode : BDDNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BDDVariableNode"/> class.
        /// </summary>
        public BDDVariableNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BDDVariableNode"/> class.
        /// </summary>
        /// <param name="variable">
        /// The variable.
        /// </param>
        /// <param name="highNode">
        /// The high node.
        /// </param>
        /// <param name="lowNode">
        /// The low node.
        /// </param>
        public BDDVariableNode(int variable, BDDNode highNode, BDDNode lowNode) : base(highNode, lowNode)
        {
            this.Variable = variable;
        }
    }
}
