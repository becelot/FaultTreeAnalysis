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

        public override FaultTreeNode transform(FaultTreeTerminalNode terminal)
        {
            if (terminal.Label == this.ID)
            {
                return createNode(new FaultTreeLiteralNode(terminal, value));
            } else
            {
                return createNode(new FaultTreeTerminalNode(terminal));
            }
        }
    }
}
