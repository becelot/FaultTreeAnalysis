using FaultTreeAnalysis.BDD.BDDTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaultTreeAnalysis.FaultTree;
using FaultTreeAnalysis.FaultTree.Tree;
using FaultTreeAnalysis.FaultTree.Transformer;

namespace FaultTreeAnalysis.BDD
{
	public class BDDFactoryComponentConnection : BDDFactory
	{
		public static BDDFactory getInstance()
		{
			if (_instance == null)
			{
				_instance = new BDDFactoryComponentConnection();
			}
			return _instance;
		}

		private enum BDDOperator
		{
			BDD_OPERATOR_AND,
			BDD_OPERATOR_OR
		}

		private BDDNode app(BDDNodeFactory nodeFactory, BDDOperator op, Dictionary<Tuple<BDDNode, BDDNode>, BDDNode> G, BDDNode u1, BDDNode u2)
		{
			BDDNode u = null;
			if (G.ContainsKey(new Tuple<BDDNode, BDDNode>(u1, u2)))
			{
				return G[new Tuple<BDDNode, BDDNode>(u1, u2)];
			}
			else if (u1.GetType() == typeof(BDDTerminalNode) && u2.GetType() == typeof(BDDTerminalNode))
			{
				if (op == BDDOperator.BDD_OPERATOR_AND)
				{
					u = nodeFactory.createNode(((BDDTerminalNode)u1).Value && ((BDDTerminalNode)u2).Value);
				}
				else
				{
					u = nodeFactory.createNode(((BDDTerminalNode)u1).Value || ((BDDTerminalNode)u2).Value);
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
				u = nodeFactory.createNode(u1.Variable, app(nodeFactory, op, G, u1.HighNode, u2.HighNode), app(nodeFactory, op, G, u1.LowNode, u2.LowNode));
			}
			else if (u1.Variable < u2.Variable)
			{
				u = nodeFactory.createNode(u1.Variable, app(nodeFactory, op, G, u1.HighNode, u2), app(nodeFactory, op, G, u1.LowNode, u2));
			}
			else
			{
				u = nodeFactory.createNode(u2.Variable, app(nodeFactory, op, G, u1, u2.HighNode), app(nodeFactory, op, G, u1, u2.LowNode));
			}
			G.Add(new Tuple<BDDNode, BDDNode>(u1, u2), u);
			return u;
		}

		private BDDNode apply(BDDNodeFactory nodeFactory, BDDOperator op, BDDNode u1, BDDNode u2)
		{
			if (u1 == null)
			{
				return u2;
			}
			Dictionary<Tuple<BDDNode, BDDNode>, BDDNode> G = new Dictionary<Tuple<BDDNode, BDDNode>, BDDNode>();
			return app(nodeFactory, op, G, u1, u2);
		}

		private BDDNode createBDD(FaultTreeNode node, BDDNodeFactory nodeFactory)
		{
			if (node.GetType() == typeof(FaultTreeTerminalNode))
			{
				return nodeFactory.createNode(((FaultTreeTerminalNode)node).Label);
			}

			BDDOperator op = BDDOperator.BDD_OPERATOR_AND;
			if (node.GetType() == typeof(FaultTreeOrGateNode))
			{
				op = BDDOperator.BDD_OPERATOR_OR;
			}

			Console.WriteLine(node.ID);

			BDDNode current = null;
			foreach (FaultTreeNode tn in node.Childs)
			{
				BDDNode n = createBDD(tn, nodeFactory);
				if (n.Variable == 5)
				{
					Console.WriteLine("Here");
				}
				current = apply(nodeFactory, op, current, n);
			}

			return current;
		}

		public override BDDNode createBDD(FaultTree.FaultTree ft)
		{
			int maxBasicEventNumber = ft.reduce<int>(new MaxTerminalTransformer());
			BDDNodeFactory nodeFactory = new BDDNodeFactory();
			nodeFactory.setBasicEventCount(maxBasicEventNumber);
			return this.createBDD(ft.Root, nodeFactory);
		}
	}
}
