using FaultTreeAnalysis.FaultTree.Tree;
using Graphviz4Net.Graphs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using FaultTreeAnalysis.GUI.ViewModel;

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
			    if (n is FaultTreeTerminalNode) continue;
				graph.AddVertex(n);

				foreach (FaultTreeNode c in n.Childs)
				{
                    graph.AddEdge(new StyledEdge<FaultTreeNode>(n, c));
				}
			}

		    var terminalNodes = nodes.OfType<FaultTreeTerminalNode>();

		    var components = faultTree.MarkovChain.GetComponents(terminalNodes);

		    var componentList = components as IList<IEnumerable<FaultTreeTerminalNode>> ?? components.ToList();

		    foreach (var component in componentList.Where(c => c.Count() == 1))
		    {
		        graph.AddVertex(component.ToList()[0]);
		    }

		    foreach (var sub in componentList.Where(c => c.Count() > 1))
		    {
		        SubGraph<FaultTreeNode> subGraph = new SubGraph<FaultTreeNode>();
		        sub.ToList().ForEach(vertex => subGraph.AddVertex(vertex));
                graph.AddSubGraph(subGraph);
		    }

            
		    var markovEdges = faultTree.MarkovChain.GetAllEdges();
		    foreach (var mEdge in markovEdges)
		    {
                var edge = new Edge<FaultTreeNode>(mEdge.Item1, mEdge.Item3, new Arrow())
                {
                    Label = faultTree.MarkovChain[mEdge.Item1, mEdge.Item3].ToString(CultureInfo.InvariantCulture)
                };
                graph.AddEdge(edge);
            }

			return graph;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
