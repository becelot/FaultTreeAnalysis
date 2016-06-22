using System;

namespace FaultTreeAnalysis.BDD.BDDTree
{
    public class BDDTerminalNode : BDDNode
    {
        public Boolean Value { get; set; }

        public BDDTerminalNode() :base() { }
        public BDDTerminalNode(Boolean Value)
        {
            this.Value = Value;
        }
	}
}
