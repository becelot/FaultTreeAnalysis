// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaxTerminalTransformer.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    using System.Collections.Generic;
    using System.Linq;

    using FaultTreeAnalysis.FaultTree.Tree;

    /// <summary>
    /// Finds max variable name from terminal node.
    /// </summary>
    public class MaxTerminalTransformer : FaultTreeTransformer<int>
    {
        /// <summary>
        /// The transform.
        /// </summary>
        /// <param name="terminal">
        /// The terminal.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int Transform(FaultTreeTerminalNode terminal)
        {
            return terminal.Label;
        }

        /// <summary>
        /// The transform.
        /// </summary>
        /// <param name="literal">
        /// The literal.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int Transform(FaultTreeLiteralNode literal)
        {
            return int.MinValue;
        }

        /// <summary>
        /// The transform.
        /// </summary>
        /// <param name="gate">
        /// The gate.
        /// </param>
        /// <param name="childs">
        /// The childs.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int Transform(FaultTreeAndGateNode gate, List<int> childs)
        {
            return childs.DefaultIfEmpty(0).Max();
		}

        /// <summary>
        /// The transform.
        /// </summary>
        /// <param name="gate">
        /// The gate.
        /// </param>
        /// <param name="childs">
        /// The childs.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public override int Transform(FaultTreeOrGateNode gate, List<int> childs)
        {
            return childs.DefaultIfEmpty(0).Max();
		}
    }
}
