using FaultTreeAnalysis.BDD.BDDTree;
using FaultTreeAnalysis.FaultTree.Transformer;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.BDD
{
	public class BDDFactoryRecursive : BDDFactory
	{
		public static BDDFactory GetInstance()
		{
			if (Instance == null)
			{
				Instance = new BDDFactoryRecursive();
			}
			return Instance;
		}

		private BDDNode createBDD(FaultTree.FaultTree ft, BDDNodeFactory nodeFactory)
		{
			int nextVariable = ft.Reduce(new MinTerminalTransformer());
			if (nextVariable == int.MaxValue)
			{
				//ft should consist of only terminal node
				return nodeFactory.CreateNode(((FaultTreeLiteralNode)ft.Root).Value);
			}

			FaultTree.FaultTree high = ft.DeepCopy().Replace(nextVariable, true).Simplify();
			FaultTree.FaultTree low = ft.DeepCopy().Replace(nextVariable, false).Simplify();

			BDDNode highNode = createBDD(high, nodeFactory);
			BDDNode lowNode = createBDD(low, nodeFactory);

			return nodeFactory.CreateNode(nextVariable, highNode, lowNode);
		}

		public override BDDNode CreateBDD(FaultTree.FaultTree ft)
		{
			//ft = new FaultTree.FaultTree(ft.reduce<FaultTreeNode>(new AddTransformer(1)));
			BDDNodeFactory nodeFactory = new BDDNodeFactory();
			return createBDD(ft, nodeFactory);
		}
	}
}
