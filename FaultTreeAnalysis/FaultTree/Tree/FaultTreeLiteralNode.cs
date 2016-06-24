using FaultTreeAnalysis.FaultTree.Transformer;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    public class FaultTreeLiteralNode : FaultTreeNode
    {
        public bool Value { get; set; }

        public FaultTreeLiteralNode()
        { }
        public FaultTreeLiteralNode(int id) : base(id) { }
        public FaultTreeLiteralNode(int id, bool value) : base(id) { Value = value; }
        public FaultTreeLiteralNode(FaultTreeTerminalNode terminal, bool value) : this(terminal.ID, value) { }
        public FaultTreeLiteralNode(FaultTreeLiteralNode literal) : this(literal.ID, literal.Value) { }

        public override T Reduce<T>(FaultTreeTransformer<T> tr)
        {
            return tr.Transform(this);
        }
    }
}
