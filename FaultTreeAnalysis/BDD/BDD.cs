using FaultTreeAnalysis.BDD.BDDTree;
using System.Runtime.Serialization;

namespace FaultTreeAnalysis.BDD
{
	[DataContract(Name = "BDD")]
	public class BDD
    {
		[DataMember()]
		public BDDNode Root { get; set; }

		public BDD()
		{

		}

		public BDD(BDDNode root)
		{
			this.Root = root;
		}

		public static implicit operator BDD(FaultTree.FaultTree ft)
		{
			return new BDD(BDDFactory.getComponentConnectionInstance().createBDD(ft));
		}
	}
}
