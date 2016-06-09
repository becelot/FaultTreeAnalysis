using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.BDD.BDDTree
{
    public class BDDTerminalNode : BDDNode
    {
        public Boolean Value { get; set; }

        public BDDTerminalNode() :base() { }
        public BDDTerminalNode(Boolean Value)
        {
            this.Value = Value;
        }
    }
}
