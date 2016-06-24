using System;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class ReplaceTransformer : TreeTransformer
    {
        private readonly int label;
        private readonly Boolean value;

        public ReplaceTransformer(int label, Boolean value)
        {
            this.label = label;
            this.value = value;
        }

        public override FaultTreeNode Transform(FaultTreeTerminalNode terminal)
        {
            return terminal.Label == label ? CreateNode(new FaultTreeLiteralNode(terminal, value)) : CreateNode(new FaultTreeTerminalNode(terminal));
        }
    }
}
