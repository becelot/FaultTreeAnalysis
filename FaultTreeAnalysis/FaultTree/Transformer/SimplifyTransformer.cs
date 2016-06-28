// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimplifyTransformer.cs" company="RWTH-Aachen">
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
    /// Simplifies Fault Tree on basis of <see cref="FaultTreeLiteralNode"/>
    /// </summary>
    public class SimplifyTransformer : TreeTransformer
    {
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
            bool all = true;
            foreach (FaultTreeNode c in childs)
            {
                if (c.GetType() == typeof(FaultTreeLiteralNode))
                {
                    if (((FaultTreeLiteralNode)c).Value == false)
                    {
                        return this.CreateNode(new FaultTreeLiteralNode(gate.ID, false));
                    }
                }
                else
                {
                    all = false;
                }
            }

            return all ? this.CreateNode(new FaultTreeLiteralNode(gate.ID, true)) : base.Transform(gate, childs);
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
            bool all = true;
            foreach (FaultTreeNode c in childs)
            {
                if (c.GetType() == typeof(FaultTreeLiteralNode))
                {
                    if (((FaultTreeLiteralNode)c).Value)
                    {
                        return this.CreateNode(new FaultTreeLiteralNode(gate.ID, true));
                    }
                }
                else
                {
                    all = false;
                }
            }

            return all ? this.CreateNode(new FaultTreeLiteralNode(gate.ID, false)) : base.Transform(gate, childs);
        }
    }
}
