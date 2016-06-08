using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class ReplaceTransformer : TreeTransformer
    {
        int ID;
        Boolean value;

        public ReplaceTransformer(int ID, Boolean value)
        {
            this.ID = ID;
            this.value = value;
        }

        public override FaultTreeNode transform(FaultTreeLiteralNode literal)
        {
            return new FaultTreeLiteralNode(literal);
        }

        public override FaultTreeNode transform(FaultTreeTerminalNode terminal)
        {
            if (terminal.Label == this.ID)
            {
                return new FaultTreeLiteralNode(terminal, value);
            } else
            {
                return new FaultTreeTerminalNode(terminal);
            }
        }

        public override FaultTreeNode transform(FaultTreeOrGateNode gate, List<FaultTreeNode> childs)
        {
            return new FaultTreeOrGateNode(gate.ID, childs);
        }

        public override FaultTreeNode transform(FaultTreeAndGateNode gate, List<FaultTreeNode> childs)
        {
            return new FaultTreeAndGateNode(gate.ID, childs);
        }
    }
}
