using System.Collections.Generic;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class TreeTransformer : FaultTreeTransformer<FaultTreeNode>
    {
        private readonly Dictionary<int, FaultTreeNode> referenceSafety = new Dictionary<int, FaultTreeNode>();

        protected FaultTreeNode createNode(FaultTreeNode node)
        {
            if (referenceSafety.ContainsKey(node.ID))
            {
                return referenceSafety[node.ID];
            }
            referenceSafety.Add(node.ID, node);
            return node;
        }

        public override FaultTreeNode transform(FaultTreeLiteralNode literal)
        {
            return createNode(new FaultTreeLiteralNode(literal));
        }

        public override FaultTreeNode transform(FaultTreeTerminalNode terminal)
        {
            return createNode(new FaultTreeTerminalNode(terminal));
        }

        public override FaultTreeNode transform(FaultTreeOrGateNode gate, List<FaultTreeNode> childs)
        {
            return createNode(new FaultTreeOrGateNode(gate.ID, childs));
        }

        public override FaultTreeNode transform(FaultTreeAndGateNode gate, List<FaultTreeNode> childs)
        {
            return createNode(new FaultTreeAndGateNode(gate.ID, childs));
        }
    }
}
