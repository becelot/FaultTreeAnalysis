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
        private static BDDFactory _instance = null;

        private BDDFactory() { }

        public static BDDFactory getInstance()
        {
            if (_instance == null)
            {
                _instance = new BDDFactory();
            }
            return _instance;
        }

        private BDD createBDD(FaultTree.FaultTree ft, int label)
        {
            if (label > 6)
            {
                if (ft.Root.GetType() == typeof(FaultTreeLiteralNode))
                {
                    Console.WriteLine("Seems about right");
                } else
                {
                    Console.WriteLine(ft.Root.GetType().ToString());
                }
                return new BDD(null, null);
            }

            FaultTree.FaultTree high = ft.deepCopy().replace(label, true).simplify();
            FaultTree.FaultTree low = ft.deepCopy().replace(label, false).simplify();

            BDD highNode = createBDD(high, label+1);
            BDD lowNode = createBDD(low, label+1);

            return new BDD(highNode, lowNode);
        } 

        public BDD createBDD(FaultTree.FaultTree ft)
        {
            return createBDD(ft, 1);
        }
    }
}
