using FaultTreeAnalysis.FaultTree.Tree;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
	public class AddTransformer : TreeTransformer
	{
		private readonly int factor;

		public AddTransformer(int factor)
		{
			this.factor = factor;
		}

		public override FaultTreeNode Transform(FaultTreeTerminalNode terminal)
		{
			return CreateNode(new FaultTreeTerminalNode(terminal.ID, terminal.Label + factor));
		}
	}
}
