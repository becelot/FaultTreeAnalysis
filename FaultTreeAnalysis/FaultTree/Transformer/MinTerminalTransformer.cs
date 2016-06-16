using FaultTreeAnalysis.FaultTree.Tree;
using System.Collections.Generic;
using System.Linq;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class MinTerminalTransformer : FaultTreeTransformer<int>
    {
        public override int transform(FaultTreeTerminalNode terminal)
        {
            return terminal.Label;
        }

        public override int transform(FaultTreeLiteralNode literal)
        {
            return int.MaxValue;
        }

        public override int transform(FaultTreeAndGateNode gate, List<int> childs)
        {
            return childs.Min();
        }

        public override int transform(FaultTreeOrGateNode gate, List<int> childs)
        {
            return childs.Min();
        }
    }
}
