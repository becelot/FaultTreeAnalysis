using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class ReplaceTransformer : TreeTransformer
    {
        private readonly int label;
        private readonly bool value;

        public ReplaceTransformer(int label, bool value)
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
