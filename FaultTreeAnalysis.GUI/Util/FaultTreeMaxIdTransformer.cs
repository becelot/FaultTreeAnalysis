using FaultTreeAnalysis.FaultTree.Transformer;
using System.Collections.Generic;
using System.Linq;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.GUI.Util
{
    public class FaultTreeMaxIdTransformer : FaultTreeTransformer<int>
    {
        public override int Transform(FaultTreeTerminalNode terminal)
        {
            return terminal.ID;
        }

        public override int Transform(FaultTreeLiteralNode literal)
        {
            return literal.ID;
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
