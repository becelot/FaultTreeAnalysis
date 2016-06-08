﻿using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Runtime.Serialization;
using FaultTreeAnalysis.FaultTree.Transformer;

namespace FaultTreeAnalysis.FaultTree
{
    [DataContract(Name = "FaultTree")]
    public class FaultTree
    {
        [DataMember()]
        public FaultTreeNode Root { get; set; }

        public FaultTree()
        {

        }

        public FaultTree(FaultTreeNode root)
        {
            this.Root = root;
        }

        public FaultTree reduce<T>(FaultTreeTransformer<FaultTreeNode> tr)
        {
            return new FaultTree( Root.reduce(tr) );
        }

        public FaultTree deepCopy()
        {
            return reduce<FaultTreeNode>(new DeepCopyTransformer());
        }

        public FaultTree replace(int label, Boolean value)
        {
            return reduce<FaultTreeNode>(new ReplaceTransformer(label, value));
        }

        public FaultTree simplify()
        {
            return reduce<FaultTreeNode>(new SimplifyTransformer());
        }
    }
}
