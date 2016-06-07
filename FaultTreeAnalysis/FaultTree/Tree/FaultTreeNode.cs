using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    [DataContract(Name="FaultTreeNode" )]
    public abstract class FaultTreeNode
    {
        [DataMember()]
        public int ID { get; set; }

        [DataMember()]
        public List<FaultTreeNode> Childs { get; set; }

        public FaultTreeNode() { Childs = new List<FaultTreeNode>(); }

        public FaultTreeNode(int ID) { this.ID = ID; }
    }
}
