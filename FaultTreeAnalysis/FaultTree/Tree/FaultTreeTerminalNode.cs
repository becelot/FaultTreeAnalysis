using System.Runtime.Serialization;
using FaultTreeAnalysis.FaultTree.Transformer;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    public class FaultTreeTerminalNode : FaultTreeNode
    {
        [DataMember]
        public int Label { get; set; }

        public FaultTreeTerminalNode()
        {

        }

        public FaultTreeTerminalNode(int id, int label)
        {
            ID = id;
            Label = label;
        }

        public FaultTreeTerminalNode(FaultTreeTerminalNode terminal) : this(terminal.ID, terminal.Label) { }

        public override T reduce<T>(FaultTreeTransformer<T> tr)
        {
            return tr.transform(this);
        }
    }
}
