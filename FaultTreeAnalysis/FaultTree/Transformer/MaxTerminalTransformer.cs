using System.Collections.Generic;
using System.Linq;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class MaxTerminalTransformer : FaultTreeTransformer<int>
    {
        public override int Transform(FaultTreeTerminalNode terminal)
        {
            return terminal.Label;
        }

        public override int Transform(FaultTreeLiteralNode literal)
        {
            return int.MinValue;
        }

        public override int Transform(FaultTreeAndGateNode gate, List<int> childs)
        {
            return childs.DefaultIfEmpty(int.MinValue).Max();
		}

        public override int Transform(FaultTreeOrGateNode gate, List<int> childs)
        {
            return childs.DefaultIfEmpty(int.MinValue).Max();
		}
    }
}
