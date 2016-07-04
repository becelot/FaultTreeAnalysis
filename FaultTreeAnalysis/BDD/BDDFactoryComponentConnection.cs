// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BDDFactoryComponentConnection.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.BDD
{
    using System;
    using System.Collections.Generic;

    using FaultTreeAnalysis.BDD.BDDTree;
    using FaultTreeAnalysis.FaultTree.Transformer;
    using FaultTreeAnalysis.FaultTree.Tree;

    /// <summary>
    /// The bdd factory using component connection.
    /// </summary>
    public class BDDFactoryComponentConnection : BDDFactory
	{
        /// <summary>
        /// The singleton instance.
        /// </summary>
        /// <returns>
        /// The <see cref="BDDFactory"/>.
        /// </returns>
        public static BDDFactory GetInstance()
		{
		    return instance ?? (instance = new BDDFactoryComponentConnection());
		}

        /// <summary>
        /// The bdd operator.
        /// </summary>
        private enum BDDOperator
		{
			BDD_OPERATOR_AND,
			BDD_OPERATOR_OR
		}

        /// <summary>
        /// Recursive helper for "Apply". Applies operation "op" to BDD "u1" and "u2" using a lookup table "g" for efficience (dynamic programming).
        /// </summary>
        /// <param name="nodeFactory">
        /// The node factory.
        /// </param>
        /// <param name="op">
        /// The op.
        /// </param>
        /// <param name="g">
        /// The g.
        /// </param>
        /// <param name="u1">
        /// The first BDD represented by root node.
        /// </param>
        /// <param name="u2">
        /// The second BDD represented by root node.
        /// </param>
        /// <returns>
        /// The <see cref="BDDNode"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Default throw.
        /// </exception>
        private BDDNode App(BDDNodeFactory nodeFactory, BDDOperator op, Dictionary<Tuple<BDDNode, BDDNode>, BDDNode> g, BDDNode u1, BDDNode u2)
		{
			BDDNode u;
			if (g.ContainsKey(new Tuple<BDDNode, BDDNode>(u1, u2)))
			{
				return g[new Tuple<BDDNode, BDDNode>(u1, u2)];
			}

		    if (u1.GetType() == typeof(BDDTerminalNode) && u2.GetType() == typeof(BDDTerminalNode))
		    {
		        switch (op)
		        {
		            case BDDOperator.BDD_OPERATOR_AND:
		                u = nodeFactory.CreateNode(((BDDTerminalNode)u1).Value && ((BDDTerminalNode)u2).Value);
		                break;
		            case BDDOperator.BDD_OPERATOR_OR:
                        u = nodeFactory.CreateNode(((BDDTerminalNode)u1).Value || ((BDDTerminalNode)u2).Value);
                        break;
		            default:
		                throw new ArgumentOutOfRangeException(nameof(op), op, null);
		        }
		    }
		    else if (u1.Variable == u2.Variable)
		    {
		        u = nodeFactory.CreateNode(u1.Variable, this.App(nodeFactory, op, g, u1.HighNode, u2.HighNode), this.App(nodeFactory, op, g, u1.LowNode, u2.LowNode));
		    }
		    else if (u1.Variable < u2.Variable)
		    {
		        u = nodeFactory.CreateNode(u1.Variable, this.App(nodeFactory, op, g, u1.HighNode, u2), this.App(nodeFactory, op, g, u1.LowNode, u2));
		    }
		    else
		    {
		        u = nodeFactory.CreateNode(u2.Variable, this.App(nodeFactory, op, g, u1, u2.HighNode), this.App(nodeFactory, op, g, u1, u2.LowNode));
		    }

		    g.Add(new Tuple<BDDNode, BDDNode>(u1, u2), u);
		    return u;
		}

        /// <summary>
        /// Applies operation "op" to BDD "u1" and "u2"
        /// </summary>
        /// <param name="nodeFactory">
        /// The node factory.
        /// </param>
        /// <param name="op">
        /// The operation.
        /// </param>
        /// <param name="u1">
        /// The first BDD represented by root node.
        /// </param>
        /// <param name="u2">
        /// The second BDD represented by root node.
        /// </param>
        /// <returns>
        /// The <see cref="BDDNode"/>.
        /// </returns>
        private BDDNode Apply(BDDNodeFactory nodeFactory, BDDOperator op, BDDNode u1, BDDNode u2)
	    {
	        if (u1 == null)
	        {
	            return u2;
	        }

	        var g = new Dictionary<Tuple<BDDNode, BDDNode>, BDDNode>();
	        return this.App(nodeFactory, op, g, u1, u2);
	    }

        /// <summary>
        /// Helper method for recursive BDD construction from FaultTree.
        /// </summary>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="nodeFactory">
        /// The node factory.
        /// </param>
        /// <returns>
        /// The <see cref="BDDNode"/>.
        /// </returns>
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
	            BDDNode n = this.CreateBDD(tn, nodeFactory);
	            current = this.Apply(nodeFactory, op, current, n);
	        }

	        return current;
	    }

        /// <summary>
        /// Converts Fault Tree to BDD.
        /// </summary>
        /// <param name="ft">
        /// The ft.
        /// </param>
        /// <returns>
        /// The <see cref="BDDNode"/>.
        /// </returns>
        public override BDDNode CreateBDD(FaultTree.FaultTree ft)
        {
            int maxBasicEventNumber = ft.MarkovChain.InitialDistribution.Count + 1;
	        BDDNodeFactory nodeFactory = new BDDNodeFactory();
	        nodeFactory.SetBasicEventCount(maxBasicEventNumber);
	        return this.CreateBDD(ft.Root, nodeFactory);
	    }
	}
}
