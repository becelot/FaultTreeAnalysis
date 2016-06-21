namespace FaultTreeAnalysis.BDD.BDDTree
{
    public abstract class BDDNode
    {
        public BDDNode HighNode { get; set; }
        public BDDNode LowNode { get; set; }

		public int Variable { get; set; }

        public BDDNode() { }
        public BDDNode(BDDNode HighNode, BDDNode LowNode)
        {
            this.HighNode = HighNode;
            this.LowNode = LowNode;
        } 
    }
}
