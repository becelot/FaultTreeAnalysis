using FaultTreeAnalysis.FaultTree.Tree;
using System;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class ReplaceTransformer : TreeTransformer
    {
        int Label;
        Boolean value;

        public ReplaceTransformer(int Label, Boolean value)
        {
            this.Label = Label;
            this.value = value;
        }

        public override FaultTreeNode transform(FaultTreeTerminalNode terminal)
        {
            if (terminal.Label == this.Label)
            {
                return createNode(new FaultTreeLiteralNode(terminal, value));
            } else
            {
                return createNode(new FaultTreeTerminalNode(terminal));
            }
        }
    }
}
