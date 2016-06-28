// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultTreeGateNode.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.FaultTree.Tree
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The fault tree gate node.
    /// </summary>
    [DataContract(Name = "FaultTreeGateNode")]
    public abstract class FaultTreeGateNode : FaultTreeNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeGateNode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected FaultTreeGateNode(int id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeGateNode"/> class.
        /// </summary>
        protected FaultTreeGateNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeGateNode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="childs">
        /// The childs.
        /// </param>
        protected FaultTreeGateNode(int id, List<FaultTreeNode> childs)
            : base(id, childs)
        {
        }
    }
}
