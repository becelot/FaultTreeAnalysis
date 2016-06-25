using FaultTreeAnalysis.FaultTree.Tree;
using Graphviz4Net.Graphs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace FaultTreeAnalysis.GUI.Converters
{
	public class GraphFaultTreeConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var graph = new Graph<FaultTreeNode>();
			if (value == null)
				return graph;

			Console.WriteLine(@"Converting tree");

			FaultTree.FaultTree faultTree = ((FaultTree.FaultTree)value);
			List<FaultTreeNode> nodes = faultTree.Traverse().ToList();

			foreach (FaultTreeNode n in nodes)
			{
				graph.AddVertex(n);

				foreach (FaultTreeNode c in n.Childs)
				{
                    graph.AddEdge(new StyledEdge<FaultTreeNode>(n, c));
				}
			}


			var faultTreeTerminalNodes = graph.GetAllVertices().OfType<FaultTreeTerminalNode>().OrderBy(n => n.Label).ToList();
			int count = faultTreeTerminalNodes.Count();
			for (int i = 0; i < count; i++)
			{
				for (int j = 0; j < count; j++)
				{
					if (faultTree.GeneratorMatrix[i, j] == 0) continue;

					var edge = new Edge<FaultTreeNode>(faultTreeTerminalNodes[i - 1], faultTreeTerminalNodes[j - 1], new Arrow())
					{
						Label = ((FaultTree.FaultTree) value).GeneratorMatrix[i, j].ToString(CultureInfo.InvariantCulture)
					};
					graph.AddEdge(edge);
				}
			}

			return graph;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
