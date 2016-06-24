using FaultTreeAnalysis.BDD.BDDTree;
using FaultTreeAnalysis.FaultTree.Transformer;
using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.BDD
{
	public class BDDFactoryRecursive : BDDFactory
	{
		public static BDDFactory getInstance()
		{
			if (_instance == null)
			{
				_instance = new BDDFactoryRecursive();
			}
			return _instance;
		}

		private BDDNode createBDD(FaultTree.FaultTree ft, BDDNodeFactory nodeFactory)
		{
			int nextVariable = ft.reduce<int>(new MinTerminalTransformer());
			if (nextVariable == int.MaxValue)
			{
				//ft should consist of only terminal node
				return nodeFactory.createNode(((FaultTreeLiteralNode)ft.Root).Value);
			}

			FaultTree.FaultTree high = ft.deepCopy().replace(nextVariable, true).simplify();
			FaultTree.FaultTree low = ft.deepCopy().replace(nextVariable, false).simplify();

			BDDNode highNode = createBDD(high, nodeFactory);
			BDDNode lowNode = createBDD(low, nodeFactory);

			return nodeFactory.createNode(nextVariable, highNode, lowNode);
		}

		public override BDDNode createBDD(FaultTree.FaultTree ft)
		{
			//ft = new FaultTree.FaultTree(ft.reduce<FaultTreeNode>(new AddTransformer(1)));
			BDDNodeFactory nodeFactory = new BDDNodeFactory();
			return createBDD(ft, nodeFactory);
		}
	}
}
