using Graphviz4Net.Graphs;
using System.Collections.Generic;
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
