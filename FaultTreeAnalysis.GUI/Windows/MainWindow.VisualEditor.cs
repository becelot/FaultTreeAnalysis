namespace FaultTreeAnalysis.GUI.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    using FaultTreeAnalysis.FaultTree;
    using FaultTreeAnalysis.FaultTree.Tree;
    using FaultTreeAnalysis.GUI.Util;

    using Graphviz4Net.Graphs;
    using Graphviz4Net.WPF.ViewModels;

    using MahApps.Metro.Controls.Dialogs;

    public partial class MainWindow
    {
        private List<Type> validSourceElements = new List<Type>();
        private List<Type> validDestinationElements = new List<Type>();

        public enum VisualEditorMode
        {
			MODE_VIEW_ONLY, 
            MODE_ADD_AND_GATE, 
            MODE_ADD_OR_GATE, 
            MODE_ADD_BASIC_EVENT, 
            MODE_ADD_GATE_CONNECTION, 
            MODE_ADD_MARKOV_CHAIN, 
			MODE_REMOVE_CONTENT
        }

        public VisualEditorMode EditorMode { get; private set; } = VisualEditorMode.MODE_VIEW_ONLY;

        private void EditorDownGate(FrameworkElement sender)
        {
            FaultTreeNode vertex;

            switch(this.EditorMode)
            {
                case VisualEditorMode.MODE_ADD_AND_GATE:
                    vertex = new FaultTreeAndGateNode(this.viewModel.FaultTree.NextId());
                    break;
                case VisualEditorMode.MODE_ADD_OR_GATE:
                    vertex = new FaultTreeOrGateNode(this.viewModel.FaultTree.NextId());
                    break;
                case VisualEditorMode.MODE_ADD_BASIC_EVENT:
                    vertex = new FaultTreeTerminalNode(this.viewModel.FaultTree.NextId(), this.viewModel.FaultTree.NextBasicEvent());
                    break;
                default:
                    return;
            }

            this.viewModel.NewEdgeStart = (FaultTreeNode)sender.DataContext;
            this.viewModel.NewEdgeEnd = vertex;
            this.viewModel.CreateEdge();
            this.EditorMode = VisualEditorMode.MODE_VIEW_ONLY;
        }

        private void EditorDownConnection(FrameworkElement sender, MouseEventArgs e)
        {
            var canvas = this.FaultTreeGridView;

            var canvasLine = canvas.Children.OfType<Line>().LastOrDefault();
            if (canvasLine == null)
            {
                if (!this.validSourceElements.Contains(sender.DataContext.GetType()))
                {
                    return;
                }

                var startPoint = e.GetPosition(canvas);
                var line = new Line
                {
                    Stroke = Brushes.Red, 
                    StrokeThickness = 6, 
                    X1 = startPoint.X, 
                    Y1 = startPoint.Y, 
                    X2 = startPoint.X, 
                    Y2 = startPoint.Y
                };

                canvas.Children.Add(line);

                this.viewModel.NewEdgeStart = (FaultTreeNode)sender.DataContext;
            }
            else
            {
                if (!this.validDestinationElements.Contains(sender.DataContext.GetType()))
                {
                    return;
                }

                canvas.Children.Remove(canvasLine);

                this.viewModel.NewEdgeEnd = (FaultTreeNode)sender.DataContext;
                this.viewModel.CreateEdge();
            }
        }

	    private async void EditorDownRemove(Grid sender)
	    {
		    FaultTreeNode node = (FaultTreeNode) sender.DataContext;

		    var t = await MessageDialogs.ShowWarningAsync("Are you sure you want to remove this node?");
		    if (t != MessageDialogResult.Affirmative) return;
		    var parents = this.viewModel.FaultTree.Traverse()?.Where(n => n.Childs.Contains(node)).ToList();

			parents?.ForEach(parent => parent.Childs.Remove(node));
			parents?.ForEach(parent => parent.Childs.AddRange(node.Childs));
			parents?.ForEach(parent => parent.Childs = parent.Childs.Distinct().ToList());

	        this.viewModel.RaisePropertyChanged("FaultTree");
	    }

        private void FaultTreeZoomControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            switch (this.EditorMode)
            {
                case VisualEditorMode.MODE_ADD_GATE_CONNECTION:
				case VisualEditorMode.MODE_ADD_MARKOV_CHAIN:
                    this.EditorDownConnection((Grid)sender, e);
                    break;
                case VisualEditorMode.MODE_ADD_AND_GATE:
                case VisualEditorMode.MODE_ADD_OR_GATE:
                case VisualEditorMode.MODE_ADD_BASIC_EVENT:
                    this.EditorDownGate((Grid)sender);
                    break;
				case VisualEditorMode.MODE_REMOVE_CONTENT:
                    this.EditorDownRemove((Grid)sender);
					break;
            }
        }

        private void FaultTreeZoomControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.validDestinationElements.Contains(((Grid)sender).DataContext.GetType()))
            {
                return;
            }

            var canvas = this.FaultTreeGridView;

            var line = canvas.Children.OfType<Line>().LastOrDefault();
            if (line == null) return;


            var endPoint = e.GetPosition(canvas);
            var directed = endPoint - new Point(line.X1, line.Y1);
            directed.Normalize();
            line.X2 = endPoint.X - directed.X * 2;
            line.Y2 = endPoint.Y - directed.Y * 2;
        }

        private void AddGateConnection(object sender, RoutedEventArgs e)
        {
            this.validSourceElements = new List<Type> { typeof(FaultTreeAndGateNode), typeof(FaultTreeOrGateNode) };
            this.validDestinationElements = new List<Type> { typeof(FaultTreeAndGateNode), typeof(FaultTreeOrGateNode), typeof(FaultTreeTerminalNode) };
            this.EditorMode = VisualEditorMode.MODE_ADD_GATE_CONNECTION;
        }

        private void AddMarkovChain(object sender, RoutedEventArgs e)
        {
            this.validSourceElements = new List<Type> { typeof(FaultTreeTerminalNode) };
            this.validDestinationElements = new List<Type> { typeof(FaultTreeTerminalNode) };
            this.EditorMode = VisualEditorMode.MODE_ADD_MARKOV_CHAIN;
        }

	    private void PathMouseDown(object sender, MouseEventArgs e)
	    {
			var edge = (Edge<FaultTreeNode>)((EdgeViewModel)((FrameworkElement)sender).DataContext).Edge;

		    if (edge.Source is FaultTreeTerminalNode)
		    {
			    return;
		    }

		    FaultTreeNode node;
		    switch (this.EditorMode)
		    {
			    case VisualEditorMode.MODE_ADD_AND_GATE:
					node = new FaultTreeAndGateNode(this.viewModel.FaultTree.NextId());
					break;
			    case VisualEditorMode.MODE_ADD_OR_GATE:
					node = new FaultTreeOrGateNode(this.viewModel.FaultTree.NextId());
					break;
			    default:
				    return;
		    }

		    edge.Source.Childs.Add(node);
		    edge.Source.Childs.Remove(edge.Destination);
		    node.Childs.Add(edge.Destination);

	        this.viewModel.RaisePropertyChanged("FaultTree");
	        this.EditorMode = VisualEditorMode.MODE_VIEW_ONLY;
	    }

	    private void RemoveComponent(object sender, RoutedEventArgs e)
	    {
	        this.validSourceElements = new List<Type> {typeof(FaultTreeAndGateNode), typeof(FaultTreeOrGateNode), typeof(FaultTreeTerminalNode)};
	        this.EditorMode = VisualEditorMode.MODE_REMOVE_CONTENT;
	    }

	    private void AddAndGate(object sender, RoutedEventArgs e)
	    {
		    if (!this.GraphLayout.Graph.Vertices.Any())
		    {
		        this.viewModel.FaultTree = new FaultTree(new FaultTreeAndGateNode(0));
		    }
		    else
		    {
		        this.EditorMode = VisualEditorMode.MODE_ADD_AND_GATE;
		        this.validSourceElements = new List<Type> {typeof(FaultTreeOrGateNode), typeof(FaultTreeAndGateNode)};
		    }
	    }

	    private void AddOrGate(object sender, RoutedEventArgs e)
	    {
		    if (!this.GraphLayout.Graph.Vertices.Any())
		    {
		        this.viewModel.FaultTree = new FaultTree(new FaultTreeOrGateNode(0));
		    }
		    else
		    {
		        this.EditorMode = VisualEditorMode.MODE_ADD_OR_GATE;
		        this.validSourceElements = new List<Type> {typeof(FaultTreeOrGateNode), typeof(FaultTreeAndGateNode)};
		    }
	    }

	    private void AddBasicEvent(object sender, RoutedEventArgs e)
	    {
		    if (!this.GraphLayout.Graph.Vertices.Any()) return;

	        this.EditorMode = VisualEditorMode.MODE_ADD_BASIC_EVENT;
	        this.validSourceElements = new List<Type> {typeof(FaultTreeOrGateNode), typeof(FaultTreeAndGateNode)};
	    }

	    private void ChangeRateClick(object sender, MouseButtonEventArgs e)
	    {
		    if (e.ClickCount >= 2)
		    {
			    var t = (Edge<FaultTreeNode>) ((EdgeLabelViewModel) ((FrameworkElement) sender).DataContext).Edge;
		        this.viewModel.NewEdgeStart = t.Source;
		        this.viewModel.NewEdgeEnd = t.Destination;
		        this.viewModel.CreateEdge();
		    }
	    }
    }
}
