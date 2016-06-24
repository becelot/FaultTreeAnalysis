using System.Collections.Generic;
using System.Linq;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class MinTerminalTransformer : FaultTreeTransformer<int>
    {
        public override int Transform(FaultTreeTerminalNode terminal)
        {
            return terminal.Label;
        }

        public override int Transform(FaultTreeLiteralNode literal)
        {
            return int.MaxValue;
        }

        public override int Transform(FaultTreeAndGateNode gate, List<int> childs)
        {
            return childs.Min();
        }

        public override int Transform(FaultTreeOrGateNode gate, List<int> childs)
        {
            return childs.Min();
        }
    }
}
