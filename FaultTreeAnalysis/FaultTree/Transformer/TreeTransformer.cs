// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeTransformer.cs" company="RWTH-Aachen">
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
    /// <see cref="FaultTreeTransformer{T}"/> wrapper for creating new Fault Trees from reduction.
    /// </summary>
    public class TreeTransformer : FaultTreeTransformer<FaultTreeNode>
    {
        /// <summary>
        /// The reference dictionary. Prevents nodes from creating multiple copies of the same node.
        /// </summary>
        private readonly Dictionary<int, FaultTreeNode> referenceSafety = new Dictionary<int, FaultTreeNode>();

        /// <summary>
        /// Yields a <see cref="FaultTreeNode"/>. Creates it if it does not exist, otherwise a reference to an existing node is returned.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <returns>
        /// The <see cref="FaultTreeNode"/>.
        /// </returns>
        protected FaultTreeNode CreateNode(FaultTreeNode node)
        {
            if (this.referenceSafety.ContainsKey(node.ID))
            {
                return this.referenceSafety[node.ID];
            }

            this.referenceSafety.Add(node.ID, node);
            return node;
        }

        /// <summary>
        /// The transform.
        /// </summary>
        /// <param name="literal">
        /// The literal.
        /// </param>
        /// <returns>
        /// The <see cref="FaultTreeNode"/>.
        /// </returns>
        public override FaultTreeNode Transform(FaultTreeLiteralNode literal)
        {
            return this.CreateNode(new FaultTreeLiteralNode(literal));
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
            return this.CreateNode(new FaultTreeTerminalNode(terminal));
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
        /// The <see cref="FaultTreeNode"/>.
        /// </returns>
        public override FaultTreeNode Transform(FaultTreeOrGateNode gate, List<FaultTreeNode> childs)
        {
            return this.CreateNode(new FaultTreeOrGateNode(gate.ID, childs));
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
        /// The <see cref="FaultTreeNode"/>.
        /// </returns>
        public override FaultTreeNode Transform(FaultTreeAndGateNode gate, List<FaultTreeNode> childs)
        {
            return this.CreateNode(new FaultTreeAndGateNode(gate.ID, childs));
        }
    }
}
