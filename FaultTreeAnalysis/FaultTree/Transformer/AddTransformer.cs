// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AddTransformer.cs" company="RWTH-Aachen">
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
    /// The add transformer.
    /// </summary>
    public class AddTransformer : TreeTransformer
	{
        /// <summary>
        /// Factor to be added to terminal nodes.
        /// </summary>
        private readonly int factor;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddTransformer"/> class.
        /// </summary>
        /// <param name="factor">
        /// The factor.
        /// </param>
        public AddTransformer(int factor)
		{
			this.factor = factor;
		}

        /// <summary>
        /// The transform.
        /// </summary>
        /// <param name="terminal">
        /// Terminal node to be transformed.
        /// </param>
        /// <returns>
        /// The <see cref="FaultTreeNode"/>.
        /// </returns>
        public override FaultTreeNode Transform(FaultTreeTerminalNode terminal)
		{
			return this.CreateNode(new FaultTreeTerminalNode(terminal.ID, terminal.Label + this.factor));
		}
	}
}
