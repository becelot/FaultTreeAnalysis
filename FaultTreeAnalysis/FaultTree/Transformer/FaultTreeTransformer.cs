using System.Collections.Generic;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public abstract class FaultTreeTransformer<T>
    {
        public abstract T Transform(FaultTreeLiteralNode literal);

        public abstract T Transform(FaultTreeTerminalNode terminal);

        public abstract T Transform(FaultTreeOrGateNode gate, List<T> childs);

        public abstract T Transform(FaultTreeAndGateNode gate, List<T> childs);
    }
}
