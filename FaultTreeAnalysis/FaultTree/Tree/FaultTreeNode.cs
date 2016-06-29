// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultTreeNode.cs" company="RWTH-Aachen">
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

    using FaultTreeAnalysis.FaultTree.Transformer;

    /// <summary>
    /// Abstract Fault Tree Node for other nodes to inherit.
    /// </summary>
    [DataContract(Name = "FaultTreeNode")]
    public abstract class FaultTreeNode
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        [DataMember]
        public int ID { get; set; }

        /// <summary>
        /// Gets or sets the childs.
        /// </summary>
        [DataMember]
        public List<FaultTreeNode> Childs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeNode"/> class.
        /// </summary>
        protected FaultTreeNode()
        {
            this.Childs = new List<FaultTreeNode>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeNode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        protected FaultTreeNode(int id)
            : this()
        {
            this.ID = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeNode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="childs">
        /// The childs.
        /// </param>
        protected FaultTreeNode(int id, List<FaultTreeNode> childs)
        {
            this.ID = id;
            this.Childs = childs;
        }

        /// <summary>
        /// Reducer that has to be overwritten by concrete implementations.
        /// </summary>
        /// <param name="tr">
        /// The tr.
        /// </param>
        /// <typeparam name="T">
        /// FaultTree will be reduced to type T.
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>T</cref>
        ///     </see>
        ///     .
        /// </returns>
        public abstract T Reduce<T>(FaultTreeTransformer<T> tr);
    }
}
