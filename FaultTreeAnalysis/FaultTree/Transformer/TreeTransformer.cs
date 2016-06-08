using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public interface TreeTransformer
    {
        FaultTreeNode transform(FaultTreeTerminalNode terminal);
        FaultTreeNode transform(FaultTreeLiteralNode literal);
        FaultTreeNode transform(FaultTreeAndGateNode gate, List<FaultTreeNode> childs);
        FaultTreeNode transform(FaultTreeOrGateNode gate, List<FaultTreeNode> childs);
    }
}
