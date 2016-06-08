using FaultTreeAnalysis.FaultTree.Transformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    public class FaultTreeTerminalNode : FaultTreeNode
    {
        [DataMember()]
        public int Label { get; set; }

        public FaultTreeTerminalNode() : base()
        {

        }

        public FaultTreeTerminalNode(int id, int label)
        {
            this.ID = id;
            this.Label = label;
        }

        public FaultTreeTerminalNode(FaultTreeTerminalNode terminal) : this(terminal.ID, terminal.Label) { }

        public override T reduce<T>(FaultTreeTransformer<T> tr)
        {
            return tr.transform(this);
        }
    }
}
