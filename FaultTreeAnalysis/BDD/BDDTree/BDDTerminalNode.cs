using System;
using System.Runtime.Serialization;

namespace FaultTreeAnalysis.BDD.BDDTree
{
	[DataContract(Name = "BDDTerminalNode")]
    public class BDDTerminalNode : BDDNode
    {
		[DataMember()]
		public Boolean Value { get; set; }

        public BDDTerminalNode() :base() { }
        public BDDTerminalNode(Boolean Value)
        {
            this.Value = Value;
        }
	}
}
