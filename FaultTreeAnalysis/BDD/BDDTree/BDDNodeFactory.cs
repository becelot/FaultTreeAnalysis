using System;
using System.Collections.Generic;

namespace FaultTreeAnalysis.BDD.BDDTree
{
    public class BDDNodeFactory
    {
		private readonly BDDTerminalNode terminalZero;
		private readonly BDDTerminalNode terminalOne;

		private readonly Dictionary<Tuple<int, BDDNode, BDDNode>, BDDNode> h;


		public BDDNodeFactory()
		{
		    terminalZero = new BDDTerminalNode(false) {Variable = 0};
		    terminalOne = new BDDTerminalNode(true) {Variable = -1};

		    h = new Dictionary<Tuple<int, BDDNode, BDDNode>, BDDNode>();
		}



		public BDDNode CreateNode(int value)
		{
			return CreateNode(value, terminalOne, terminalZero);
		}

        public BDDNode CreateNode(int value, BDDNode highNode, BDDNode lowNode)
        {
            if (highNode == lowNode)
			{
				return lowNode;
			}
            if (h.ContainsKey(new Tuple<int, BDDNode, BDDNode>(value, highNode, lowNode)))
            {
                return h[new Tuple<int, BDDNode, BDDNode>(value, highNode, lowNode)];
            }
            BDDNode n = new BDDVariableNode(value, highNode, lowNode);
            h.Add(new Tuple<int, BDDNode, BDDNode>(value, highNode, lowNode), n);
            return n;
        }

        public BDDNode CreateNode(bool value)
        {
			return value ? terminalOne : terminalZero;
        }

		internal void SetBasicEventCount(int maxBasicEventNumber)
		{
			terminalOne.Variable = maxBasicEventNumber + 1;
			terminalZero.Variable = maxBasicEventNumber + 2;
		}
	}
}
