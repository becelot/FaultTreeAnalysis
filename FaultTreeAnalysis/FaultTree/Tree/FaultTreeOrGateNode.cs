// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultTreeOrGateNode.cs" company="RWTH-Aachen">
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
    /// The fault tree or gate node.
    /// </summary>
    [DataContract(Name = "FaultTreeOrGateNode")]
    public class FaultTreeOrGateNode : FaultTreeGateNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeOrGateNode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public FaultTreeOrGateNode(int id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeOrGateNode"/> class.
        /// </summary>
        public FaultTreeOrGateNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeOrGateNode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="childs">
        /// The childs.
        /// </param>
        public FaultTreeOrGateNode(int id, List<FaultTreeNode> childs)
            : base(id, childs)
        {
        }

        /// <summary>
        /// Concrete implementation of reduce.
        /// </summary>
        /// <param name="tr">
        /// The <see cref="FaultTreeTransformer{T}"/>
        /// </param>
        /// <typeparam name="T">
        /// The type the Fault Tree is reduced to.
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>T</cref>
        ///     </see>
        ///     .
        /// </returns>
        public override T Reduce<T>(FaultTreeTransformer<T> tr)
        {
            List<T> l = this.Childs.Select(c => c.Reduce(tr)).ToList();

            return tr.Transform(this, l);
        }
    }
}
