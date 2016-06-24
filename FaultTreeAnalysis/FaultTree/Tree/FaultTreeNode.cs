using FaultTreeAnalysis.FaultTree.Transformer;
using System.Collections.Generic;
using System.Runtime.Serialization;

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

        public FaultTreeNode(int ID) : this() { this.ID = ID; }

        public FaultTreeNode(int ID, List<FaultTreeNode> childs)
        {
            this.ID = ID;
            this.Childs = childs;
        }

        public abstract T reduce<T>(FaultTreeTransformer<T> tr);
    }
}
