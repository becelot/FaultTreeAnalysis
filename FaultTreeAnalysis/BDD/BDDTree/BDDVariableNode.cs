using System.Runtime.Serialization;

namespace FaultTreeAnalysis.BDD.BDDTree
{
	[DataContract(Name = "BDDVariableNode")]
    public class BDDVariableNode : BDDNode
    {
        public BDDVariableNode()
        { }
        public BDDVariableNode(int variable, BDDNode highNode, BDDNode lowNode) : base(highNode, lowNode)
        {
            this.Variable = variable;
        }
    }
}
