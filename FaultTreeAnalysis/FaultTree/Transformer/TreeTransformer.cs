using System.Collections.Generic;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class TreeTransformer : FaultTreeTransformer<FaultTreeNode>
    {
        private readonly Dictionary<int, FaultTreeNode> referenceSafety = new Dictionary<int, FaultTreeNode>();

        protected FaultTreeNode CreateNode(FaultTreeNode node)
        {
            if (referenceSafety.ContainsKey(node.ID))
            {
                return referenceSafety[node.ID];
            }
            referenceSafety.Add(node.ID, node);
            return node;
        }

        public override FaultTreeNode Transform(FaultTreeLiteralNode literal)
        {
            return CreateNode(new FaultTreeLiteralNode(literal));
        }

        public override FaultTreeNode Transform(FaultTreeTerminalNode terminal)
        {
            return CreateNode(new FaultTreeTerminalNode(terminal));
        }

        public override FaultTreeNode Transform(FaultTreeOrGateNode gate, List<FaultTreeNode> childs)
        {
            return CreateNode(new FaultTreeOrGateNode(gate.ID, childs));
        }

        public override FaultTreeNode Transform(FaultTreeAndGateNode gate, List<FaultTreeNode> childs)
        {
            return CreateNode(new FaultTreeAndGateNode(gate.ID, childs));
        }
    }
}
