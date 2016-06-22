using FaultTreeAnalysis.FaultTree.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree.Transformer
{
	public class AddTransformer : TreeTransformer
	{
		private int Factor;

		public AddTransformer(int factor)
		{
			this.Factor = factor;
		}

		public override FaultTreeNode transform(FaultTreeTerminalNode terminal)
		{
			return createNode(new FaultTreeTerminalNode(terminal.ID, terminal.Label + Factor));
		}
	}
}
