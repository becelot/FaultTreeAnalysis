using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
	public class AddTransformer : TreeTransformer
	{
		private int Factor;

		public AddTransformer(int factor)
		{
			Factor = factor;
		}

		public override FaultTreeNode transform(FaultTreeTerminalNode terminal)
		{
			return createNode(new FaultTreeTerminalNode(terminal.ID, terminal.Label + Factor));
		}
	}
}
