using System;
using System.Collections.Generic;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class SimplifyTransformer : TreeTransformer
    {
        public override FaultTreeNode Transform(FaultTreeAndGateNode gate, List<FaultTreeNode> childs)
        {
            Boolean all = true;
            foreach (FaultTreeNode c in childs)
            {
                if (c.GetType() == typeof(FaultTreeLiteralNode))
                {
                    if ( ((FaultTreeLiteralNode)c).Value == false)
                    {
                        return CreateNode(new FaultTreeLiteralNode(gate.ID, false));
                    }
                } else
                {
                    all = false;
                }
            }

            if (all)
            {
                return CreateNode(new FaultTreeLiteralNode(gate.ID, true));
            }

            return base.Transform(gate, childs);
        }

        public override FaultTreeNode Transform(FaultTreeOrGateNode gate, List<FaultTreeNode> childs)
        {
            Boolean all = true;
            foreach (FaultTreeNode c in childs)
            {
                if (c.GetType() == typeof(FaultTreeLiteralNode))
                {
                    if (((FaultTreeLiteralNode)c).Value)
                    {
                        return CreateNode(new FaultTreeLiteralNode(gate.ID, true));
                    }
                } else
                {
                    all = false;
                }
            }

            if (all)
            {
                return CreateNode(new FaultTreeLiteralNode(gate.ID, false));
            }

            return base.Transform(gate, childs);
        }
    }
}
