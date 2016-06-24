using System;
using System.Collections.Generic;
using FaultTreeAnalysis.BDD.BDDTree;
using FaultTreeAnalysis.FaultTree.Transformer;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.BDD
{
	public class BDDFactoryComponentConnection : BDDFactory
	{
		public static BDDFactory GetInstance()
		{
			if (Instance == null)
			{
				Instance = new BDDFactoryComponentConnection();
			}
			return Instance;
		}

		private enum BDDOperator
		{
			BDD_OPERATOR_AND,
			BDD_OPERATOR_OR
		}

		private BDDNode App(BDDNodeFactory nodeFactory, BDDOperator op, Dictionary<Tuple<BDDNode, BDDNode>, BDDNode> g, BDDNode u1, BDDNode u2)
		{
			BDDNode u = null;
			if (g.ContainsKey(new Tuple<BDDNode, BDDNode>(u1, u2)))
			{
				return g[new Tuple<BDDNode, BDDNode>(u1, u2)];
			}
		    if (u1.GetType() == typeof(BDDTerminalNode) && u2.GetType() == typeof(BDDTerminalNode))
		    {
		        if (op == BDDOperator.BDD_OPERATOR_AND)
		        {
		            u = nodeFactory.CreateNode(((BDDTerminalNode)u1).Value && ((BDDTerminalNode)u2).Value);
		        }
		        else
		        {
		            u = nodeFactory.CreateNode(((BDDTerminalNode)u1).Value || ((BDDTerminalNode)u2).Value);
		        }
		    } /*
			else if (u1.GetType() == typeof(BDDTerminalNode))
			{
				if (op == BDDOperator.BDD_OPERATOR_AND)
				{
					if (((BDDTerminalNode)u1).Value == false)
					{
						u = nodeFactory.createNode(false);
					}
					else
					{
						u = u2;
					}
				}
				else
				{
					if (((BDDTerminalNode)u1).Value == true)
					{
						u = nodeFactory.createNode(true);
					}
					else
					{
						u = u2;
					}
				}
			}
			else if (u2.GetType() == typeof(BDDTerminalNode))
			{
				if (op == BDDOperator.BDD_OPERATOR_AND)
				{
					if (((BDDTerminalNode)u2).Value == false)
					{
						u = nodeFactory.createNode(false);
					}
					else
					{
						u = u1;
					}
				}
				else
				{
					if (((BDDTerminalNode)u2).Value == true)
					{
						u = nodeFactory.createNode(true);
					}
					else
					{
						u = u1;
					}
				}
			} */
		    else if (u1.Variable == u2.Variable)
		    {
		        u = nodeFactory.CreateNode(u1.Variable, App(nodeFactory, op, g, u1.HighNode, u2.HighNode), App(nodeFactory, op, g, u1.LowNode, u2.LowNode));
		    }
		    else if (u1.Variable < u2.Variable)
		    {
		        u = nodeFactory.CreateNode(u1.Variable, App(nodeFactory, op, g, u1.HighNode, u2), App(nodeFactory, op, g, u1.LowNode, u2));
		    }
		    else
		    {
		        u = nodeFactory.CreateNode(u2.Variable, App(nodeFactory, op, g, u1, u2.HighNode), App(nodeFactory, op, g, u1, u2.LowNode));
		    }
		    g.Add(new Tuple<BDDNode, BDDNode>(u1, u2), u);
			return u;
		}

		private BDDNode Apply(BDDNodeFactory nodeFactory, BDDOperator op, BDDNode u1, BDDNode u2)
		{
			if (u1 == null)
			{
				return u2;
			}
			Dictionary<Tuple<BDDNode, BDDNode>, BDDNode> g = new Dictionary<Tuple<BDDNode, BDDNode>, BDDNode>();
			return App(nodeFactory, op, g, u1, u2);
		}

		private BDDNode CreateBDD(FaultTreeNode node, BDDNodeFactory nodeFactory)
		{
			if (node.GetType() == typeof(FaultTreeTerminalNode))
			{
				return nodeFactory.CreateNode(((FaultTreeTerminalNode)node).Label);
			}

			BDDOperator op = BDDOperator.BDD_OPERATOR_AND;
			if (node.GetType() == typeof(FaultTreeOrGateNode))
			{
				op = BDDOperator.BDD_OPERATOR_OR;
			}

			BDDNode current = null;
			foreach (FaultTreeNode tn in node.Childs)
			{
				BDDNode n = CreateBDD(tn, nodeFactory);
				current = Apply(nodeFactory, op, current, n);
			}

			return current;
		}

		public override BDDNode CreateBDD(FaultTree.FaultTree ft)
		{
			int maxBasicEventNumber = ft.Reduce(new MaxTerminalTransformer());
			BDDNodeFactory nodeFactory = new BDDNodeFactory();
			nodeFactory.SetBasicEventCount(maxBasicEventNumber);
			return CreateBDD(ft.Root, nodeFactory);
		}
	}
}
