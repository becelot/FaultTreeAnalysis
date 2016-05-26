using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree
{
    [Serializable()]
    public class FaultTree
    {
        public FaultTreeNode Root { get; set; }

        public FaultTree()
        {

        }

        public FaultTree(FaultTreeNode root)
        {
            this.Root = root;
        }
    }
}
