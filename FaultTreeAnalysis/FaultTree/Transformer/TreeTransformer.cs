using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public abstract class TreeTransformer
    {
        public virtual FaultTreeNode transform(FaultTreeLiteralNode literal)
        {
            return new FaultTreeLiteralNode(literal);
        }

        public virtual FaultTreeNode transform(FaultTreeTerminalNode terminal)
        {
            return new FaultTreeTerminalNode(terminal);
        }

        public virtual FaultTreeNode transform(FaultTreeOrGateNode gate, List<FaultTreeNode> childs)
        {
            return new FaultTreeOrGateNode(gate.ID, childs);
        }

        public virtual FaultTreeNode transform(FaultTreeAndGateNode gate, List<FaultTreeNode> childs)
        {
            return new FaultTreeAndGateNode(gate.ID, childs);
        }
    }
}
