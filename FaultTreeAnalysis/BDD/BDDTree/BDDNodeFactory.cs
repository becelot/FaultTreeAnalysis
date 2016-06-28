// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BDDNodeFactory.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.BDD.BDDTree
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The bdd node factory.
    /// </summary>
    public class BDDNodeFactory
    {
        /// <summary>
        /// The zero terminal.
        /// </summary>
        private readonly BDDTerminalNode terminalZero;

        /// <summary>
        /// The one terminal.
        /// </summary>
        private readonly BDDTerminalNode terminalOne;

        /// <summary>
        /// Map for finding equal nodes that can be merged. (Suggested by Anderson)
        /// </summary>
        private readonly Dictionary<Tuple<int, BDDNode, BDDNode>, BDDNode> h;

        /// <summary>
        /// Initializes a new instance of the <see cref="BDDNodeFactory"/> class.
        /// </summary>
        public BDDNodeFactory()
		{
            this.terminalZero = new BDDTerminalNode(false) { Variable = 0 };
            this.terminalOne = new BDDTerminalNode(true) { Variable = -1 };

            this.h = new Dictionary<Tuple<int, BDDNode, BDDNode>, BDDNode>();
		}

        /// <summary>
        /// Creates new BDD from value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="BDDNode"/>.
        /// </returns>
        public BDDNode CreateNode(int value)
		{
			return this.CreateNode(value, this.terminalOne, this.terminalZero);
		}

        /// <summary>
        /// Creates a new node or returns reference to existing node if it shares the same properties.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="highNode">
        /// The high node.
        /// </param>
        /// <param name="lowNode">
        /// The low node.
        /// </param>
        /// <returns>
        /// The <see cref="BDDNode"/>.
        /// </returns>
        public BDDNode CreateNode(int value, BDDNode highNode, BDDNode lowNode)
        {
            if (highNode == lowNode)
			{
				return lowNode;
			}

            if (this.h.ContainsKey(new Tuple<int, BDDNode, BDDNode>(value, highNode, lowNode)))
            {
                return this.h[new Tuple<int, BDDNode, BDDNode>(value, highNode, lowNode)];
            }

            BDDNode n = new BDDVariableNode(value, highNode, lowNode);
            this.h.Add(new Tuple<int, BDDNode, BDDNode>(value, highNode, lowNode), n);
            return n;
        }

        /// <summary>
        /// Easy access to the zero and one terminal node.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="BDDNode"/>.
        /// </returns>
        public BDDNode CreateNode(bool value)
        {
			return value ? this.terminalOne : this.terminalZero;
        }

        /// <summary>
        /// Used for setting terminalOne and terminalZero to appropriate values for Apply algorithm.
        /// </summary>
        /// <param name="maxBasicEventNumber">
        /// The max basic event number.
        /// </param>
        internal void SetBasicEventCount(int maxBasicEventNumber)
		{
            this.terminalOne.Variable = maxBasicEventNumber + 1;
            this.terminalZero.Variable = maxBasicEventNumber + 2;
		}
	}
}
