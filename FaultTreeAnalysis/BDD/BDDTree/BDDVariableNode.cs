namespace FaultTreeAnalysis.BDD.BDDTree
{
    public class BDDVariableNode : BDDNode
    {
        public BDDVariableNode() : base() { }
        public BDDVariableNode(int Variable, BDDNode HighNode, BDDNode LowNode) : base(HighNode, LowNode)
        {
            this.Variable = Variable;
        }
    }
}
