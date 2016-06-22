﻿
namespace Graphviz4Net.WPF.ViewModels
{
    using Graphs;

    public class EdgeLabelViewModel
    {
        public EdgeLabelViewModel(string label, IEdge edge)
        {
            this.Label = label;
            this.Edge = edge;
        }

        public string Label { get; private set; }

        public IEdge Edge { get; private set; }
    }
}
