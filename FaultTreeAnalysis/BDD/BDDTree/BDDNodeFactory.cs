﻿using System;
using System.Collections.Generic;

namespace FaultTreeAnalysis.BDD.BDDTree
{
    public class BDDNodeFactory
    {
		private BDDTerminalNode terminalZero;
		private BDDTerminalNode terminalOne;

		private Dictionary<Tuple<int, BDDNode, BDDNode>, BDDNode> H;
		private Dictionary<BDDNode, Tuple<int, BDDNode, BDDNode>> T;




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
			return this.createNode(value, terminalOne, terminalZero);
		}

        public BDDNode createNode(int value, BDDNode HighNode, BDDNode LowNode)
        {
			if (HighNode.Variable == LowNode.Variable)
			{
				return LowNode;
			} else if (H.ContainsKey(new Tuple<int, BDDNode, BDDNode>(value, HighNode, LowNode)))
			{
				return H[new Tuple<int, BDDNode, BDDNode>(value, HighNode, LowNode)];
			} else
			{
				BDDNode n = new BDDVariableNode(value, HighNode, LowNode);
				T.Add(n, new Tuple<int, BDDNode, BDDNode>(value, HighNode, LowNode));
				H.Add(new Tuple<int, BDDNode, BDDNode>(value, HighNode, LowNode), n);
				return n;
			}
        }

        public BDDNode createNode(Boolean value)
        {
			return value ? terminalOne : terminalZero;
        }
    }
}
