using System.Collections.Generic;
using System.Runtime.Serialization;
using FaultTreeAnalysis.BDD.BDDTree;

namespace FaultTreeAnalysis.BDD
{
	[DataContract(Name = "BinaryDecisionDiagram")]
	public class BinaryDecisionDiagram
    {
		[DataMember]
		public BDDNode Root { get; set; }

		public BinaryDecisionDiagram()
		{

		}

		public BinaryDecisionDiagram(BDDNode root)
		{
			Root = root;
		}

		public static implicit operator BinaryDecisionDiagram(FaultTree.FaultTree ft)
		{
			return new BinaryDecisionDiagram(BDDFactory.getComponentConnectionInstance().createBDD(ft));
		}

		public IEnumerable<BDDNode> flatMap()
		{
			return BDDNode.Traverse(Root);
		}
	}
}
