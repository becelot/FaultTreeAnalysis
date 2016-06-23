using FaultTreeAnalysis.FaultTree.Tree;
using Graphviz4Net.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Ink;
using System.Windows.Media;

namespace FaultTreeAnalysis.GUI
{
	public class StyledEdge<TVertex> : Edge<TVertex>
	{
		public DoubleCollection DashArray { get; set; }

		public StyledEdge(TVertex source, TVertex destination, object destinationArrow = null, object sourceArrow = null, object destinationPort = null, object sourcePort = null, IDictionary<string, string> attributes = null) : base(source, destination, destinationArrow, sourceArrow, destinationPort, sourcePort, attributes)
		{
			DashArray = new DoubleCollection(new List<double> { });
		}

		public StyledEdge(TVertex source, TVertex destination, int dashFrequency, object destinationArrow = null, object sourceArrow = null, object destinationPort = null, object sourcePort = null, IDictionary<string, string> attributes = null) : base(source, destination, destinationArrow, sourceArrow, destinationPort, sourcePort, attributes)
		{
			DashArray = new DoubleCollection(new List<double> { dashFrequency });
		}
	}
}
