using System.Runtime.Serialization;

namespace FaultTreeAnalysis.BDD.BDDTree
{
	[DataContract(Name = "BDDVariableNode")]
    public class BDDVariableNode : BDDNode
    {
        public BDDVariableNode()
        { }
        public BDDVariableNode(int Variable, BDDNode HighNode, BDDNode LowNode) : base(HighNode, LowNode)
        {
            this.Variable = Variable;
        }
    }
}
