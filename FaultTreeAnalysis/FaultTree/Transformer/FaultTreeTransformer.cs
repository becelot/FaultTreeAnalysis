using System.Collections.Generic;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public abstract class FaultTreeTransformer<T>
    {
        public abstract T transform(FaultTreeLiteralNode literal);

        public abstract T transform(FaultTreeTerminalNode terminal);

        public abstract T transform(FaultTreeOrGateNode gate, List<T> childs);

        public abstract T transform(FaultTreeAndGateNode gate, List<T> childs);
    }
}
