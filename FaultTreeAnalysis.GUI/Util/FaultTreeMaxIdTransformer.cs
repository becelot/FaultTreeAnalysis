using FaultTreeAnalysis.FaultTree.Transformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.GUI.Util
{
    public class FaultTreeMaxIdTransformer : FaultTreeTransformer<int>
    {
        public override int transform(FaultTreeTerminalNode terminal)
        {
            return terminal.ID;
        }

        public override int transform(FaultTreeLiteralNode literal)
        {
            return literal.ID;
        }

        public override int transform(FaultTreeAndGateNode gate, List<int> childs)
        {
            return childs.Max();
        }

        public override int transform(FaultTreeOrGateNode gate, List<int> childs)
        {
            return childs.Max();
        }
    }
}
