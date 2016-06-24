using System;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class ReplaceTransformer : TreeTransformer
    {
        readonly int Label;
        readonly Boolean value;

        public ReplaceTransformer(int Label, Boolean value)
        {
            this.Label = Label;
            this.value = value;
        }

        public override FaultTreeNode transform(FaultTreeTerminalNode terminal)
        {
            if (terminal.Label == Label)
            {
                return createNode(new FaultTreeLiteralNode(terminal, value));
            }
            return createNode(new FaultTreeTerminalNode(terminal));
        }
    }
}
