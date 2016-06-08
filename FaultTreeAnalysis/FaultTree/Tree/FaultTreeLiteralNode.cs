using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    public class FaultTreeLiteralNode : FaultTreeNode
    {
        public Boolean Value { get; set; }

        public FaultTreeLiteralNode() : base() { }
        public FaultTreeLiteralNode(int ID) : base(ID) { }
        public FaultTreeLiteralNode(int ID, Boolean value) : base(ID) { this.Value = value; }
        public FaultTreeLiteralNode(FaultTreeTerminalNode terminal, Boolean value) : this(terminal.ID, value) { }

        public override FaultTreeNode reduce(TreeTransformer tr)
        {
            return tr.transform(this);
        }
    }
}
