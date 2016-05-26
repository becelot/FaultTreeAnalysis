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

        public FaultTreeTerminalNode()
        {

        }

        public FaultTreeTerminalNode(int id, int label)
        {
            this.ID = id;
            this.Label = label;
        }

    }
}
