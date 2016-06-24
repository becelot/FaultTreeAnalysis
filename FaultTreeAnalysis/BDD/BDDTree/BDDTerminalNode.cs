using System.Runtime.Serialization;

namespace FaultTreeAnalysis.BDD.BDDTree
{
	[DataContract(Name = "BDDTerminalNode")]
    public class BDDTerminalNode : BDDNode
    {
		[DataMember]
		public bool Value { get; set; }

        public BDDTerminalNode()
        { }
        public BDDTerminalNode(bool value)
        {
            Value = value;
        }
	}
}
