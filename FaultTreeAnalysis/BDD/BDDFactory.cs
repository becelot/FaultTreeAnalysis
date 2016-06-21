using FaultTreeAnalysis.FaultTree.Transformer;
using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.Collections.Generic;

namespace FaultTreeAnalysis.BDD.BDDTree
{
    public class BDDFactory
    {
        private static BDDFactory _instance = null;
		private static BDDNodeFactory nodeFactory = null;

        private BDDFactory()
		{
			nodeFactory = new BDDNodeFactory();
		}

        public static BDDFactory getInstance()
        {
            if (_instance == null)
            {
                _instance = new BDDFactory();
            }
            return _instance;
        }

        public BDDNode createBDD(FaultTree.FaultTree ft)
        {
            int nextVariable = ft.reduce<int>(new MinTerminalTransformer());
            if (nextVariable == int.MaxValue)
            {
                //ft should consist of only terminal node
                return nodeFactory.createNode(((FaultTreeLiteralNode)ft.Root).Value);
            }

            FaultTree.FaultTree high = ft.deepCopy().replace(nextVariable, true).simplify();
            FaultTree.FaultTree low = ft.deepCopy().replace(nextVariable, false).simplify();

            BDDNode highNode = createBDD(high);
            BDDNode lowNode = createBDD(low);

            return nodeFactory.createNode(nextVariable, highNode, lowNode);
        }

		private enum BDDOperator
		{
			BDD_OPERATOR_AND,
			BDD_OPERATOR_OR
		}

		private BDDNode app(BDDOperator op, Dictionary<Tuple<BDDNode, BDDNode>, BDDNode> G, BDDNode u1, BDDNode u2)
		{
			BDDNode u = null;
			if (G.ContainsKey(new Tuple<BDDNode, BDDNode>(u1, u2)))
			{
				return G[new Tuple<BDDNode, BDDNode>(u1, u2)];
			} else if (u1.GetType() == typeof(BDDTerminalNode) && u2.GetType() == typeof(BDDTerminalNode))
			{
				if (op == BDDOperator.BDD_OPERATOR_AND)
				{
					u = nodeFactory.createNode(((BDDTerminalNode)u1).Value && ((BDDTerminalNode)u2).Value);
				} else
				{
					u = nodeFactory.createNode(((BDDTerminalNode)u1).Value || ((BDDTerminalNode)u2).Value);
				}
			} else if (u1.GetType() == typeof(BDDTerminalNode))
			{
				if (op == BDDOperator.BDD_OPERATOR_AND)
				{
					if (((BDDTerminalNode)u1).Value == false)
					{
						u = nodeFactory.createNode(false);
					} else
					{
						u = u2;
					}
				} else
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
			} else if (u2.GetType() == typeof(BDDTerminalNode))
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
			} else if (u1.Variable == u2.Variable)
			{
				u = nodeFactory.createNode(u1.Variable,  app(op, G, u1.HighNode, u2.HighNode), app(op, G, u1.LowNode, u2.LowNode));
			} else if (u1.Variable < u2.Variable)
			{
				u = nodeFactory.createNode(u1.Variable, app(op, G, u1.HighNode, u2), app(op, G, u1.LowNode, u2));
			} else
			{
				u = nodeFactory.createNode(u2.Variable, app(op, G, u1, u2.HighNode), app(op, G, u1, u2.LowNode));
			}
			G.Add(new Tuple<BDDNode, BDDNode>(u1, u2), u);
            return u;
		}

		private BDDNode apply(BDDOperator op, BDDNode u1, BDDNode u2)
		{
			if (u1 == null)
			{
				return u2;
			}
			Dictionary<Tuple<BDDNode, BDDNode>, BDDNode> G = new Dictionary<Tuple<BDDNode, BDDNode>, BDDNode>();
			return app(op, G, u1, u2);
		}

		public BDDNode createBDDModularized(FaultTreeNode node)
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

			BDDNode current = null;
			foreach (FaultTreeNode tn in node.Childs)
			{
				BDDNode n = createBDDModularized(tn);
				current = apply(op, current, n);
			}

			return current;
		}

		public BDDNode createBDDModularized(FaultTree.FaultTree ft)
		{
			return this.createBDDModularized(ft.Root);
		}
    }
}
