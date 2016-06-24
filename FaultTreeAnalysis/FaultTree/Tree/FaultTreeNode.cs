using System.Collections.Generic;
using System.Runtime.Serialization;
using FaultTreeAnalysis.FaultTree.Transformer;

namespace FaultTreeAnalysis.FaultTree.Tree
{
    [DataContract(Name="FaultTreeNode" )]
    public abstract class FaultTreeNode
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public List<FaultTreeNode> Childs { get; set; }

        protected FaultTreeNode() { Childs = new List<FaultTreeNode>(); }

        protected FaultTreeNode(int id) : this() { ID = id; }

        protected FaultTreeNode(int id, List<FaultTreeNode> childs)
        {
            ID = id;
            Childs = childs;
        }

        public abstract T Reduce<T>(FaultTreeTransformer<T> tr);
    }
}
