﻿using System;
using System.Collections.Generic;
using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
    public class SimplifyTransformer : TreeTransformer
    {
        public override FaultTreeNode transform(FaultTreeAndGateNode gate, List<FaultTreeNode> childs)
        {
            Boolean all = true;
            foreach (FaultTreeNode c in childs)
            {
                if (c.GetType() == typeof(FaultTreeLiteralNode))
                {
                    if ( ((FaultTreeLiteralNode)c).Value == false)
                    {
                        return createNode(new FaultTreeLiteralNode(gate.ID, false));
                    }
                } else
                {
                    all = false;
                }
            }

            if (all)
            {
                return createNode(new FaultTreeLiteralNode(gate.ID, true));
            }

            return base.transform(gate, childs);
        }

        public override FaultTreeNode transform(FaultTreeOrGateNode gate, List<FaultTreeNode> childs)
        {
            Boolean all = true;
            foreach (FaultTreeNode c in childs)
            {
                if (c.GetType() == typeof(FaultTreeLiteralNode))
                {
                    if (((FaultTreeLiteralNode)c).Value)
                    {
                        return createNode(new FaultTreeLiteralNode(gate.ID, true));
                    }
                } else
                {
                    all = false;
                }
            }

            if (all)
            {
                return createNode(new FaultTreeLiteralNode(gate.ID, false));
            }

            return base.transform(gate, childs);
        }
    }
}
