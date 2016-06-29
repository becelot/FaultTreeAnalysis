// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultTreeAndGateNode.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.FaultTree.Tree
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.Serialization;

    using FaultTreeAnalysis.FaultTree.Transformer;

    /// <summary>
    /// The fault tree and gate node.
    /// </summary>
    [DataContract(Name = "FaultTreeAndGateNode")]
    public class FaultTreeAndGateNode : FaultTreeGateNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeAndGateNode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public FaultTreeAndGateNode(int id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeAndGateNode"/> class.
        /// </summary>
        public FaultTreeAndGateNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeAndGateNode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="childs">
        /// The childs.
        /// </param>
        public FaultTreeAndGateNode(int id, List<FaultTreeNode> childs)
            : base(id, childs)
        {
        }

        /// <summary>
        /// Applies Transformer recursively on childs.
        /// </summary>
        /// <param name="tr">
        /// The TreeTransformer.
        /// </param>
        /// <typeparam name="T">
        /// Resulting type of reduction.
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>T</cref>
        ///     </see>
        ///     .
        /// </returns>
        public override T Reduce<T>(FaultTreeTransformer<T> tr)
        {
            var l = this.Childs.Select(c => c.Reduce(tr)).ToList();

            return tr.Transform(this, l);
        }
    }
}
