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
	public class BDDGraphConverter : IMultiValueConverter
	{
		public object Convert(object[] value, Type targetType, object parameter, CultureInfo culture)
		{
			var graph = new Graph<BDDNode>();

            if (!(value[0] is FaultTree.FaultTree) && !(value[1] is bool) )
            {
                return graph;
            }

            if (!(bool)value[1])
            {
                return graph;
            }
			
			if (!(value[0] is FaultTree.FaultTree))
			{
				return graph;
			}

			BDD.BinaryDecisionDiagram bdd = (FaultTree.FaultTree)value[0];

			var nodes = bdd.FlatMap();

			foreach (BDDNode n in nodes)
			{
				graph.AddVertex(n);
				if (n is BDDVariableNode)
				{
					graph.AddEdge(new StyledEdge<BDDNode>(n, n.HighNode));
					graph.AddEdge(new StyledEdge<BDDNode>(n, n.LowNode, 4));
				}
			}

			return graph;
		}

		public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
