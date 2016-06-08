using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Runtime.Serialization;

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

        public FaultTree reduce(TreeTransformer tr)
        {
            return new FaultTree( Root.reduce(tr) );
        }
    }
}
