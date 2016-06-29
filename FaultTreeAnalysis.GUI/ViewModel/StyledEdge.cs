using System.Collections.Generic;
using System.Windows.Media;
using Graphviz4Net.Graphs;

namespace FaultTreeAnalysis.GUI.ViewModel
{
	public class StyledEdge<TVertex> : Edge<TVertex>
	{
		public DoubleCollection DashArray { get; set; }

		public StyledEdge(TVertex source, TVertex destination, object destinationArrow = null, object sourceArrow = null, object destinationPort = null, object sourcePort = null, IDictionary<string, string> attributes = null) : base(source, destination, destinationArrow, sourceArrow, destinationPort, sourcePort, attributes)
		{
		    this.DashArray = new DoubleCollection(new List<double>());
		}

		public StyledEdge(TVertex source, TVertex destination, int dashFrequency, object destinationArrow = null, object sourceArrow = null, object destinationPort = null, object sourcePort = null, IDictionary<string, string> attributes = null) : base(source, destination, destinationArrow, sourceArrow, destinationPort, sourcePort, attributes)
		{
		    this.DashArray = new DoubleCollection(new List<double> { dashFrequency });
		}
	}
}
