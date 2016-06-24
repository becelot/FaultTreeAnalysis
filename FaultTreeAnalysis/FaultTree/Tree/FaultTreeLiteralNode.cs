using System;
using FaultTreeAnalysis.FaultTree.Transformer;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    public class FaultTreeLiteralNode : FaultTreeNode
    {
        public Boolean Value { get; set; }

        public FaultTreeLiteralNode()
        { }
        public FaultTreeLiteralNode(int ID) : base(ID) { }
        public FaultTreeLiteralNode(int ID, Boolean value) : base(ID) { Value = value; }
        public FaultTreeLiteralNode(FaultTreeTerminalNode terminal, Boolean value) : this(terminal.ID, value) { }
        public FaultTreeLiteralNode(FaultTreeLiteralNode literal) : this(literal.ID, literal.Value) { }

        public override T reduce<T>(FaultTreeTransformer<T> tr)
        {
            return tr.transform(this);
        }
    }
}
