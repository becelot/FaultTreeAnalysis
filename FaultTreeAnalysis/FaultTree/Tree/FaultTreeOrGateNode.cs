using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    [DataContract(Name = "FaultTreeOrGateNode")]
    public class FaultTreeOrGateNode : FaultTreeGateNode
    {
        public FaultTreeOrGateNode(int ID) : base(ID) { }
        public FaultTreeOrGateNode()  { }
    }
}
