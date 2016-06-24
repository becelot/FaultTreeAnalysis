namespace FaultTreeAnalysis.BDD.BDDTree
{
    public abstract class BDDFactory
    {
        protected static BDDFactory Instance = null;

        protected BDDFactory()
		{
			new BDDNodeFactory();
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
