﻿using System.Collections.Generic;
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

        protected FaultTreeNode(int ID) : this() { this.ID = ID; }

        protected FaultTreeNode(int ID, List<FaultTreeNode> childs)
        {
            this.ID = ID;
            Childs = childs;
        }

        public abstract T reduce<T>(FaultTreeTransformer<T> tr);
    }
}
