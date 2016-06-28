// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BDDFactoryRecursive.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.BDD
{
    using FaultTreeAnalysis.BDD.BDDTree;
    using FaultTreeAnalysis.FaultTree.Transformer;
    using FaultTreeAnalysis.FaultTree.Tree;

    /// <summary>
    /// Create <see cref="BinaryDecisionDiagram"/> from <see cref="FaultTreeAnalysis.FaultTree"/> recursively.
    /// </summary>
    public class BDDFactoryRecursive : BDDFactory
	{
        /// <summary>
        /// Singleton instance.
        /// </summary>
        /// <returns>
        /// The <see cref="BDDFactory"/>.
        /// </returns>
        public static BDDFactory GetInstance()
		{
		    return instance ?? (instance = new BDDFactoryRecursive());
		}

        /// <summary>
        /// Private helper for BDD construction.
        /// </summary>
        /// <param name="ft">
        /// The ft.
        /// </param>
        /// <param name="nodeFactory">
        /// The node factory.
        /// </param>
        /// <returns>
        /// The <see cref="BDDNode"/>.
        /// </returns>
        private BDDNode CreateBDD(FaultTree.FaultTree ft, BDDNodeFactory nodeFactory)
		{
			int nextVariable = ft.Reduce(new MinTerminalTransformer());
			if (nextVariable == int.MaxValue)
			{
			    // ft should consist of only terminal node
				return nodeFactory.CreateNode(((FaultTreeLiteralNode)ft.Root).Value);
			}

			FaultTree.FaultTree high = ft.DeepCopy().Replace(nextVariable, true).Simplify();
			FaultTree.FaultTree low = ft.DeepCopy().Replace(nextVariable, false).Simplify();

			BDDNode highNode = this.CreateBDD(high, nodeFactory);
			BDDNode lowNode = this.CreateBDD(low, nodeFactory);

			return nodeFactory.CreateNode(nextVariable, highNode, lowNode);
		}

        /// <summary>
        /// Create <see cref="BinaryDecisionDiagram"/> from <see cref="FaultTreeAnalysis.FaultTree"/>.
        /// </summary>
        /// <param name="ft">
        /// The ft.
        /// </param>
        /// <returns>
        /// The <see cref="BDDNode"/>.
        /// </returns>
        public override BDDNode CreateBDD(FaultTree.FaultTree ft)
		{
		    // ft = new FaultTree.FaultTree(ft.reduce<FaultTreeNode>(new AddTransformer(1)));
			BDDNodeFactory nodeFactory = new BDDNodeFactory();
			return this.CreateBDD(ft, nodeFactory);
		}
	}
}
