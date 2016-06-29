// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultTreeTransformer.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    using System.Collections.Generic;

    using FaultTreeAnalysis.FaultTree.Tree;

    /// <summary>
    /// The fault tree transformer.
    /// </summary>
    /// <typeparam name="T">
    /// The type the Fault Tree is reduced to.
    /// </typeparam>
    public abstract class FaultTreeTransformer<T>
    {
        /// <summary>
        /// The transform overwritten by <see cref="FaultTreeLiteralNode"/>.
        /// </summary>
        /// <param name="literal">
        /// The literal.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>T</cref>
        ///     </see>
        ///     .
        /// </returns>
        public abstract T Transform(FaultTreeLiteralNode literal);

        /// <summary>
        /// The transform overwritten by <see cref="FaultTreeTerminalNode"/>.
        /// </summary>
        /// <param name="terminal">
        /// The terminal.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>T</cref>
        ///     </see>
        ///     .
        /// </returns>
        public abstract T Transform(FaultTreeTerminalNode terminal);

        /// <summary>
        /// The transform overwritten by the <see cref="FaultTreeOrGateNode"/>.
        /// </summary>
        /// <param name="gate">
        /// The gate.
        /// </param>
        /// <param name="childs">
        /// The childs.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>T</cref>
        ///     </see>
        ///     .
        /// </returns>
        public abstract T Transform(FaultTreeOrGateNode gate, List<T> childs);

        /// <summary>
        /// The transform overwritten by the <see cref="FaultTreeAndGateNode"/>.
        /// </summary>
        /// <param name="gate">
        /// The gate.
        /// </param>
        /// <param name="childs">
        /// The childs.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>T</cref>
        ///     </see>
        ///     .
        /// </returns>
        public abstract T Transform(FaultTreeAndGateNode gate, List<T> childs);
    }
}
