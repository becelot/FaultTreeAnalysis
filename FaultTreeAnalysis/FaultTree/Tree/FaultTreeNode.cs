using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    [Serializable()]
    public abstract class FaultTreeNode
    {
        public int ID { get; set; }

        public List<FaultTreeNode> Childs { get; set; }

        public FaultTreeNode() { Childs = new List<FaultTreeNode>(); }
    }
}
