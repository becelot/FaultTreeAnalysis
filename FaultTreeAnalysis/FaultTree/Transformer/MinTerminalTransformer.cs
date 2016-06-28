// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MinTerminalTransformer.cs" company="RWTH-Aachen">
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
    /// Finds minimal variable number from <see cref="FaultTreeTerminalNode.Label"/>.
    /// </summary>
    public class MinTerminalTransformer : FaultTreeTransformer<int>
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
            return int.MaxValue;
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
            return childs.Min();
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
            return childs.Min();
        }
    }
}
