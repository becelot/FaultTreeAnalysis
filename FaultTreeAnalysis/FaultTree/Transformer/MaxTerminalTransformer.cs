using System.Collections.Generic;
using System.Linq;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class MaxTerminalTransformer : FaultTreeTransformer<int>
    {
        public override int transform(FaultTreeTerminalNode terminal)
        {
            return terminal.Label;
        }

        public override int transform(FaultTreeLiteralNode literal)
        {
            return int.MinValue;
        }

        public override int transform(FaultTreeAndGateNode gate, List<int> childs)
        {
            return childs.DefaultIfEmpty(int.MinValue).Max();
		}

        public override int transform(FaultTreeOrGateNode gate, List<int> childs)
        {
            return childs.DefaultIfEmpty(int.MinValue).Max();
		}
    }
}
