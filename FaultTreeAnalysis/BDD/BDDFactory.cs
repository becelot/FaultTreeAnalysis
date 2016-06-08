using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.BDD.BDDTree
{
    public class BDDFactory
    {
        private BDDFactory _instance = null;

        private BDDFactory() { }

        public BDDFactory getInstance()
        {
            if (_instance == null)
            {
                _instance = new BDDFactory();
            }
            return _instance;
        }

        public BDD createBDD(FaultTree.FaultTree ft)
        {
            FaultTreeNode root = ft.Root;
            /*
            FaultTreeNode high = root.deepCopy().insert(ID, 1).simplify();
            FaultTreeNode low = root.deepCopy().insert(ID, 0).simplify();

            BDD highNode = createBDD(new FaultTree.FaultTree(high));
            BDD lowNode = createBDD(new FaultTree.FaultTree(low));

            return new BDD(highNode, lowNode); */
            return null;
        }
    }
}
