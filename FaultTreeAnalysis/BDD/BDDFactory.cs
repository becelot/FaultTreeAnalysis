using FaultTreeAnalysis.BDD.BDDTree;

namespace FaultTreeAnalysis.BDD
{
    public abstract class BDDFactory
    {
        protected static BDDFactory Instance = null;

        protected BDDFactory()
		{
		}

        public static BDDFactory GetRecursiveInstance()
        {
			return BDDFactoryRecursive.GetInstance();
        }

		public static BDDFactory GetComponentConnectionInstance()
		{
			return BDDFactoryComponentConnection.GetInstance();
		}

		public abstract BDDNode CreateBDD(FaultTree.FaultTree ft);


    }
}
