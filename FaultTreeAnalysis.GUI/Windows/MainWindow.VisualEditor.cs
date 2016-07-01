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
			MODE_REMOVE_CONTENT,
            MODE_ANALYZE_CLICK
        }

        public VisualEditorMode EditorMode { get; private set; } = VisualEditorMode.MODE_VIEW_ONLY;

        private void EditorDownGate(FrameworkElement sender)
        {
            FaultTreeNode vertex;

            switch(this.EditorMode)
            {
                case VisualEditorMode.MODE_ADD_AND_GATE:
                    vertex = new FaultTreeAndGateNode(this.ViewModel.FaultTree.NextId());
                    break;
                case VisualEditorMode.MODE_ADD_OR_GATE:
                    vertex = new FaultTreeOrGateNode(this.ViewModel.FaultTree.NextId());
                    break;
                case VisualEditorMode.MODE_ADD_BASIC_EVENT:
                    FaultTreeTerminalNode node = sender.DataContext as FaultTreeTerminalNode;
                    if (node != null)
                    {
                        vertex = new FaultTreeTerminalNode(this.ViewModel.FaultTree.NextId(), this.ViewModel.FaultTree.NextBasicEvent());
                        this.ViewModel.FaultTree.MarkovChain.InitialDistribution[(FaultTreeTerminalNode)vertex] = 1.0;
                        this.ViewModel.FaultTree.MarkovChain[node, (FaultTreeTerminalNode)vertex] = 1.0;
                        this.ViewModel.FaultTree.MarkovChain[(FaultTreeTerminalNode)vertex, node] = 1.0;
                        this.ViewModel.RaisePropertyChanged("FaultTree");
                        this.EditorMode = VisualEditorMode.MODE_VIEW_ONLY;
                        return;
                    }
                    vertex = new FaultTreeTerminalNode(this.ViewModel.FaultTree.NextId(), this.ViewModel.FaultTree.NextBasicEvent());
                    this.ViewModel.FaultTree.MarkovChain.InitialDistribution[(FaultTreeTerminalNode)vertex] = 1.0;
                    break;
                default:
                    return;
            }

            this.ViewModel.NewEdgeStart = (FaultTreeNode)sender.DataContext;
            this.ViewModel.NewEdgeEnd = vertex;
            this.ViewModel.CreateEdge();
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

                this.ViewModel.NewEdgeStart = (FaultTreeNode)sender.DataContext;
            }
            else
            {
                if (!this.validDestinationElements.Contains(sender.DataContext.GetType()))
                {
                    return;
                }

                canvas.Children.Remove(canvasLine);

                this.ViewModel.NewEdgeEnd = (FaultTreeNode)sender.DataContext;
                this.ViewModel.CreateEdge();
            }
        }

	    private async void EditorDownRemove(Grid sender)
	    {
		    FaultTreeNode node = (FaultTreeNode) sender.DataContext;

	        if (Config.Instance.ShowWarningWhenRemoval)
	        {
                var t = await MessageDialogs.ShowWarningAsync("Are you sure you want to remove this node?");
                if (t != MessageDialogResult.Affirmative) return;
            }
		    
		    var parents = this.ViewModel.FaultTree.Traverse()?.Where(n => n.Childs.Contains(node)).ToList();

			parents?.ForEach(parent => parent.Childs.Remove(node));
			parents?.ForEach(parent => parent.Childs.AddRange(node.Childs));
			parents?.ForEach(parent => parent.Childs = parent.Childs.Distinct().ToList());

            if (node is FaultTreeTerminalNode)
                this.ViewModel.FaultTree.MarkovChain.RemoveVertex((FaultTreeTerminalNode)node);

	        this.ViewModel.RaisePropertyChanged("FaultTree");
	    }

        private async void FaultTreeNodePress(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount >= 2 && ((FrameworkElement)sender).DataContext is FaultTreeTerminalNode)
            {
                var probabilityInput = await MessageDialogs.ShowInputDialogAsync(
                    "Probability",
                    "Specify an initial probability!",
                    new MetroDialogSettings());

                double probabiliy;
                if (double.TryParse(probabilityInput, out probabiliy))
                {
                    if (probabiliy >= 0 && probabiliy <= 1)
                    {
                        this.ViewModel.FaultTree.MarkovChain.InitialDistribution[(FaultTreeTerminalNode)((FrameworkElement)sender).DataContext] = probabiliy;
                        this.ViewModel.RaisePropertyChanged("FaultTree");
                    }
                }
                return;
            }
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
                case VisualEditorMode.MODE_ANALYZE_CLICK:
                    FaultTree faultTree = new FaultTree((FaultTreeNode)(((Grid)sender).DataContext), this.ViewModel.FaultTree.MarkovChain);
                    this.AnalyzeFlyout.IsOpen = true;
                    this.Analyze.Initialize(faultTree);
                    break;
            }
        }

        private void AnalyzeClick(object sender, RoutedEventArgs e)
        {
            this.EditorMode = VisualEditorMode.MODE_ANALYZE_CLICK;
        }

        private void FaultTreeNodeMouseMove(object sender, MouseEventArgs e)
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
					node = new FaultTreeAndGateNode(this.ViewModel.FaultTree.NextId());
					break;
			    case VisualEditorMode.MODE_ADD_OR_GATE:
					node = new FaultTreeOrGateNode(this.ViewModel.FaultTree.NextId());
					break;
			    default:
				    return;
		    }

		    edge.Source.Childs.Add(node);
		    edge.Source.Childs.Remove(edge.Destination);
		    node.Childs.Add(edge.Destination);

	        this.ViewModel.RaisePropertyChanged("FaultTree");
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
		        this.ViewModel.FaultTree = new FaultTree(new FaultTreeAndGateNode(0));
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
		        this.ViewModel.FaultTree = new FaultTree(new FaultTreeOrGateNode(0));
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
	        this.validSourceElements = new List<Type> {typeof(FaultTreeOrGateNode), typeof(FaultTreeAndGateNode), typeof(FaultTreeTerminalNode)};
	    }

	    private void ChangeRateClick(object sender, MouseButtonEventArgs e)
	    {
		    if (e.ClickCount >= 2)
		    {
			    var t = (Edge<FaultTreeNode>) ((EdgeLabelViewModel) ((FrameworkElement) sender).DataContext).Edge;
		        this.ViewModel.NewEdgeStart = t.Source;
		        this.ViewModel.NewEdgeEnd = t.Destination;
		        this.ViewModel.CreateEdge();
		    }
	    }
    }
}
