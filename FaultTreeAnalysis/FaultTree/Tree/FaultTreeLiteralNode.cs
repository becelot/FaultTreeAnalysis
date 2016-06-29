// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultTreeLiteralNode.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.FaultTree.Tree
{
    using FaultTreeAnalysis.FaultTree.Transformer;

    /// <summary>
    /// The fault tree literal node.
    /// </summary>
    public class FaultTreeLiteralNode : FaultTreeNode
    {
        /// <summary>
        /// Gets or sets a value indicating whether value.
        /// </summary>
        public bool Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeLiteralNode"/> class.
        /// </summary>
        public FaultTreeLiteralNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeLiteralNode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public FaultTreeLiteralNode(int id)
            : base(id)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeLiteralNode"/> class.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public FaultTreeLiteralNode(int id, bool value)
            : base(id)
        {
            this.Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeLiteralNode"/> class.
        /// </summary>
        /// <param name="terminal">
        /// The terminal.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public FaultTreeLiteralNode(FaultTreeTerminalNode terminal, bool value)
            : this(terminal.ID, value)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeLiteralNode"/> class.
        /// </summary>
        /// <param name="literal">
        /// The literal.
        /// </param>
        public FaultTreeLiteralNode(FaultTreeLiteralNode literal)
            : this(literal.ID, literal.Value)
        {
        }

        /// <summary>
        /// reduces a <see cref="FaultTreeTerminalNode"/>
        /// </summary>
        /// <param name="tr">
        /// The Tree transformer.
        /// </param>
        /// <typeparam name="T">
        /// Return type.
        /// </typeparam>
        /// <returns>
        /// The <see>
        ///         <cref>T</cref>
        ///     </see>
        ///     .
        /// </returns>
        public override T Reduce<T>(FaultTreeTransformer<T> tr)
        {
            return tr.Transform(this);
        }
    }
}
