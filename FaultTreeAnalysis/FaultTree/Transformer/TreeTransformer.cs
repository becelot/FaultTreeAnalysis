using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class TreeTransformer : FaultTreeTransformer<FaultTreeNode>
    {
        private Dictionary<int, FaultTreeNode> referenceSafety = new Dictionary<int, FaultTreeNode>();

        protected FaultTreeNode createNode(FaultTreeNode node)
        {
            if (referenceSafety.ContainsKey(node.ID))
            {
                referenceSafety.Add(node.ID, node);
                return referenceSafety[node.ID];
            } else
            {
                return node;
            }
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
