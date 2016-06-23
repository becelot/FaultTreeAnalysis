﻿using Graphviz4Net.Graphs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FaultTreeAnalysis.GUI.Converters
{
	public class GraphFaultTreeConverter : IValueConverter
	{
		object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var graph = new Graph<FaultTreeGate>();
			var a = new FaultTreeAndGate() { Name = "Jonh" };
			var b = new FaultTreeOrGate() { Name = "Jonh" };
			var c = new FaultTreeAndGate() { Name = "Jonh" };

			graph.AddVertex(a);
			graph.AddVertex(b);
			graph.AddVertex(c);

			graph.AddEdge(new Edge<FaultTreeGate>(a, b));

			return graph;
		}

		object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
