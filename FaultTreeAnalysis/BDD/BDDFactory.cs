namespace FaultTreeAnalysis.BDD.BDDTree
{
    public abstract class BDDFactory
    {
        protected static BDDFactory _instance = null;
		private static BDDNodeFactory nodeFactory;

        protected BDDFactory()
		{
			nodeFactory = new BDDNodeFactory();
		}

        public static BDDFactory getRecursiveInstance()
        {
			return BDDFactoryRecursive.getInstance();
        }

		public static BDDFactory getComponentConnectionInstance()
		{
			return BDDFactoryComponentConnection.getInstance();
		}

		public abstract BDDNode createBDD(FaultTree.FaultTree ft);


    }
}
