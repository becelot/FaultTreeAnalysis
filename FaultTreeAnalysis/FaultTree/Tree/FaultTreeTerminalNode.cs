// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultTreeTerminalNode.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.FaultTree.Tree
{
    using System.Runtime.Serialization;

    using FaultTreeAnalysis.FaultTree.Transformer;

    /// <summary>
    /// The fault tree terminal node.
    /// </summary>
    public class FaultTreeTerminalNode : FaultTreeNode
    {
        /// <summary>
        /// Gets or sets the label.
        /// </summary>
        [DataMember]
        public int Label { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeTerminalNode"/> class.
        /// </summary>
        public FaultTreeTerminalNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeTerminalNode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="label">
        /// The label.
        /// </param>
        public FaultTreeTerminalNode(int id, int label)
        {
            this.ID = id;
            this.Label = label;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeTerminalNode"/> class.
        /// </summary>
        /// <param name="terminal">
        /// The terminal.
        /// </param>
        public FaultTreeTerminalNode(FaultTreeTerminalNode terminal)
            : this(terminal.ID, terminal.Label)
        {
        }

        /// <summary>
        /// Concreate implementation of reduce.
        /// </summary>
        /// <param name="tr">
        /// The <see cref="FaultTreeTransformer{T}"/>
        /// </param>
        /// <typeparam name="T">
        /// The type the Fault Tree is reduced to.
        /// </typeparam>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public override T Reduce<T>(FaultTreeTransformer<T> tr)
        {
            return tr.Transform(this);
        }
    }
}
