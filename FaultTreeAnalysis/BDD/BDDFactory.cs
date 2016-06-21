using FaultTreeAnalysis.FaultTree.Transformer;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.BDD.BDDTree
{
    public class BDDFactory
    {
        private static BDDFactory _instance = null;
		private static BDDNodeFactory nodeFactory = null;

        private BDDFactory()
		{
			nodeFactory = new BDDNodeFactory();
		}

        public static BDDFactory getInstance()
        {
            if (_instance == null)
            {
                _instance = new BDDFactory();
            }
            return _instance;
        }

        public BDDNode createBDD(FaultTree.FaultTree ft)
        {
            int nextVariable = ft.reduce<int>(new MinTerminalTransformer());
            if (nextVariable == int.MaxValue)
            {
                //ft should consist of only terminal node
                return nodeFactory.createNode(((FaultTreeLiteralNode)ft.Root).Value);
            }

            FaultTree.FaultTree high = ft.deepCopy().replace(nextVariable, true).simplify();
            FaultTree.FaultTree low = ft.deepCopy().replace(nextVariable, false).simplify();

            BDDNode highNode = createBDD(high);
            BDDNode lowNode = createBDD(low);

            return nodeFactory.createNode(nextVariable, highNode, lowNode);
        }
    }
}
