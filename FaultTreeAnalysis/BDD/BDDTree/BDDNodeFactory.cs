using System;
using System.Collections.Generic;

namespace FaultTreeAnalysis.BDD.BDDTree
{
    public class BDDNodeFactory
    {
		private readonly BDDTerminalNode terminalZero;
		private readonly BDDTerminalNode terminalOne;

		private readonly Dictionary<Tuple<int, BDDNode, BDDNode>, BDDNode> H;
		private readonly Dictionary<BDDNode, Tuple<int, BDDNode, BDDNode>> T;


		public BDDNodeFactory()
		{
			terminalZero = new BDDTerminalNode(false);
			terminalZero.Variable = 0;
			terminalOne = new BDDTerminalNode(true);
			terminalOne.Variable = -1;

			H = new Dictionary<Tuple<int, BDDNode, BDDNode>, BDDNode>();
			T = new Dictionary<BDDNode, Tuple<int, BDDNode, BDDNode>>();
		}



		public BDDNode createNode(int value)
		{
			return createNode(value, terminalOne, terminalZero);
		}

        public BDDNode createNode(int value, BDDNode HighNode, BDDNode LowNode)
        {
            if (HighNode == LowNode)
			{
				return LowNode;
			}
            if (H.ContainsKey(new Tuple<int, BDDNode, BDDNode>(value, HighNode, LowNode)))
            {
                return H[new Tuple<int, BDDNode, BDDNode>(value, HighNode, LowNode)];
            }
            BDDNode n = new BDDVariableNode(value, HighNode, LowNode);
            T.Add(n, new Tuple<int, BDDNode, BDDNode>(value, HighNode, LowNode));
            H.Add(new Tuple<int, BDDNode, BDDNode>(value, HighNode, LowNode), n);
            return n;
        }

        public BDDNode createNode(Boolean value)
        {
			return value ? terminalOne : terminalZero;
        }

		internal void setBasicEventCount(int maxBasicEventNumber)
		{
			terminalOne.Variable = maxBasicEventNumber + 1;
			terminalZero.Variable = maxBasicEventNumber + 2;
		}
	}
}
