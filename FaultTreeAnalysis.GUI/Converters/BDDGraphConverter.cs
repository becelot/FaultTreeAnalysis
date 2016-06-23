using FaultTreeAnalysis.BDD.BDDTree;
using Graphviz4Net.Graphs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FaultTreeAnalysis.GUI.Converters
{
	public class BDDGraphConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var graph = new Graph<BDDNode>();
			
			if (!(value is FaultTree.FaultTree))
			{
				return graph;
			}

			BDD.BDD bdd = (FaultTree.FaultTree)value;

			var nodes = bdd.flatMap();

			foreach (BDDNode n in nodes)
			{
				graph.AddVertex(n);
				if (n is BDDVariableNode)
				{
					graph.AddEdge(new Edge<BDDNode>(n, n.HighNode));
					graph.AddEdge(new Edge<BDDNode>(n, n.LowNode));
				}
			}

			return graph;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
