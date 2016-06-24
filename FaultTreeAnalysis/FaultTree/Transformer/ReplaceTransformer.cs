using System;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class ReplaceTransformer : TreeTransformer
    {
        private readonly int label;
        readonly Boolean value;

        public ReplaceTransformer(int label, Boolean value)
        {
            this.label = label;
            this.value = value;
        }

        public override FaultTreeNode Transform(FaultTreeTerminalNode terminal)
        {
            if (terminal.Label == label)
            {
                return CreateNode(new FaultTreeLiteralNode(terminal, value));
            }
            return CreateNode(new FaultTreeTerminalNode(terminal));
        }
    }
}
