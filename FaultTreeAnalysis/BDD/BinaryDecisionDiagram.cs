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
			return new BinaryDecisionDiagram(BDDFactory.GetComponentConnectionInstance().CreateBDD(ft));
		}

		public IEnumerable<BDDNode> FlatMap()
		{
			return BDDNode.Traverse(Root);
		}
	}
}
