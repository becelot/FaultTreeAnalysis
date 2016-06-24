using System.Collections.Generic;
using System.Runtime.Serialization;
using FaultTreeAnalysis.BDD.BDDTree;

namespace FaultTreeAnalysis.BDD
{
	[DataContract(Name = "BDD")]
	public class BDD
    {
		[DataMember]
		public BDDNode Root { get; set; }

		public BDD()
		{

		}

		public BDD(BDDNode root)
		{
			Root = root;
		}

		public static implicit operator BDD(FaultTree.FaultTree ft)
		{
			return new BDD(BDDFactory.getComponentConnectionInstance().createBDD(ft));
		}

		public IEnumerable<BDDNode> flatMap()
		{
			return BDDNode.Traverse(Root);
		}
	}
}
