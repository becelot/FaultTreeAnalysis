// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReplaceTransformer.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    using FaultTreeAnalysis.FaultTree.Tree;

    /// <summary>
    /// Replaces a <see cref="FaultTreeTerminalNode"/> with a <see cref="FaultTreeLiteralNode"/>
    /// </summary>
    public class ReplaceTransformer : TreeTransformer
    {
        /// <summary>
        /// Label of <see cref="FaultTreeTerminalNode"/> to be replaced.
        /// </summary>
        private readonly int label;

        /// <summary>
        /// Value of create <see cref="FaultTreeLiteralNode"/>.
        /// </summary>
        private readonly bool value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplaceTransformer"/> class.
        /// </summary>
        /// <param name="label">
        /// The label.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        public ReplaceTransformer(int label, bool value)
        {
            this.label = label;
            this.value = value;
        }

        /// <summary>
        /// The transform.
        /// </summary>
        /// <param name="terminal">
        /// The terminal.
        /// </param>
        /// <returns>
        /// The <see cref="FaultTreeNode"/>.
        /// </returns>
        public override FaultTreeNode Transform(FaultTreeTerminalNode terminal)
        {
            return terminal.Label == this.label ? this.CreateNode(new FaultTreeLiteralNode(terminal, this.value)) : this.CreateNode(new FaultTreeTerminalNode(terminal));
        }
    }
}
