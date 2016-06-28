using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FaultTreeAnalysis.FaultTree.Tree;
using FaultTreeAnalysis.GUI.Util;
using Graphviz4Net.Graphs;
using Graphviz4Net.WPF.ViewModels;
using MahApps.Metro.Controls.Dialogs;

namespace FaultTreeAnalysis.GUI.Windows
{
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
			MODE_INSERT_AND_GATE,
			MODE_INSERT_OR_GATE
        }

        public VisualEditorMode EditorMode { get; private set; } = VisualEditorMode.MODE_VIEW_ONLY;

        private void EditorDownGate(FrameworkElement sender)
        {
            FaultTreeNode vertex;

            switch(EditorMode)
            {
                case VisualEditorMode.MODE_ADD_AND_GATE:
                    vertex = new FaultTreeAndGateNode(viewModel.FaultTree.NextId());
                    break;
                case VisualEditorMode.MODE_ADD_OR_GATE:
                    vertex = new FaultTreeOrGateNode(viewModel.FaultTree.NextId());
                    break;
                case VisualEditorMode.MODE_ADD_BASIC_EVENT:
                    vertex = new FaultTreeTerminalNode(viewModel.FaultTree.NextId(), viewModel.FaultTree.NextBasicEvent());
                    break;
                default:
                    return;
            }

            viewModel.NewEdgeStart = (FaultTreeNode)sender.DataContext;
            viewModel.NewEdgeEnd = vertex;
            viewModel.CreateEdge();
			EditorMode = VisualEditorMode.MODE_VIEW_ONLY;
        }

        private void EditorDownConnection(FrameworkElement sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var canvas = FaultTreeGridView;

            var canvasLine = canvas.Children.OfType<Line>().LastOrDefault();
            if (canvasLine == null)
            {
                if (!validSourceElements.Contains(sender.DataContext.GetType()))
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
                    Y2 = startPoint.Y,
                };

                canvas.Children.Add(line);

                viewModel.NewEdgeStart = (FaultTreeNode)sender.DataContext;
            }
            else
            {
                if (!validDestinationElements.Contains(sender.DataContext.GetType()))
                {
                    return;
                }
                canvas.Children.Remove(canvasLine);

                viewModel.NewEdgeEnd = (FaultTreeNode)sender.DataContext;
                viewModel.CreateEdge();
            }
        }

	    private async void EditorDownRemove(Grid sender)
	    {
		    FaultTreeNode node = (FaultTreeNode) sender.DataContext;

		    var t = await MessageDialogs.ShowWarningAsync("Are you sure you want to remove this node?");
		    if (t != MessageDialogResult.Affirmative) return;
		    var parents = viewModel.FaultTree.Traverse()?.Where(n => n.Childs.Contains(node)).ToList();

			parents?.ForEach(parent => parent.Childs.Remove(node));
			parents?.ForEach(parent => parent.Childs.AddRange(node.Childs));
			parents?.ForEach(parent => parent.Childs = parent.Childs.Distinct().ToList());

			viewModel.RaisePropertyChanged("FaultTree");
	    }

        private void FaultTreeZoomControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            switch (EditorMode)
            {
                case VisualEditorMode.MODE_ADD_GATE_CONNECTION:
				case VisualEditorMode.MODE_ADD_MARKOV_CHAIN:
                    EditorDownConnection((Grid)sender, e);
                    break;
                case VisualEditorMode.MODE_ADD_AND_GATE:
                case VisualEditorMode.MODE_ADD_OR_GATE:
                case VisualEditorMode.MODE_ADD_BASIC_EVENT:
                    EditorDownGate((Grid)sender);
                    break;
				case VisualEditorMode.MODE_REMOVE_CONTENT:
					EditorDownRemove((Grid)sender);
					break;
            }
        }

        private void FaultTreeZoomControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (!validDestinationElements.Contains(((Grid)sender).DataContext.GetType()))
            {
                return;
            }
            var canvas = FaultTreeGridView;

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
            validSourceElements = new List<Type>() { typeof(FaultTreeAndGateNode), typeof(FaultTreeOrGateNode) };
            validDestinationElements = new List<Type>() { typeof(FaultTreeAndGateNode), typeof(FaultTreeOrGateNode), typeof(FaultTreeTerminalNode) };
            EditorMode = VisualEditorMode.MODE_ADD_GATE_CONNECTION;
        }

        private void AddMarkovChain(object sender, RoutedEventArgs e)
        {
            validSourceElements = new List<Type>() { typeof(FaultTreeTerminalNode) };
            validDestinationElements = new List<Type>() { typeof(FaultTreeTerminalNode) };
            EditorMode = VisualEditorMode.MODE_ADD_MARKOV_CHAIN;
        }

	    private void PathMouseDown(object sender, System.Windows.Input.MouseEventArgs e)
	    {
			var edge = (Edge<FaultTreeNode>)((EdgeViewModel)((FrameworkElement)sender).DataContext).Edge;

		    if (edge.Source is FaultTreeTerminalNode)
		    {
			    return;
		    }

		    FaultTreeNode node;
		    switch (EditorMode)
		    {
			    case VisualEditorMode.MODE_INSERT_AND_GATE:
					node = new FaultTreeAndGateNode(viewModel.FaultTree.NextId());
					break;
			    case VisualEditorMode.MODE_INSERT_OR_GATE:
					node = new FaultTreeOrGateNode(viewModel.FaultTree.NextId());
					break;
			    default:
				    return;
		    }

		    edge.Source.Childs.Add(node);
		    edge.Source.Childs.Remove(edge.Destination);
		    node.Childs.Add(edge.Destination);

		    viewModel.RaisePropertyChanged("FaultTree");
		    EditorMode = VisualEditorMode.MODE_VIEW_ONLY;
	    }


	    private void InsertOrGate(object sneder, RoutedEventArgs e)
	    {
			EditorMode = VisualEditorMode.MODE_INSERT_OR_GATE;
		}

	    private void InsertAndGate(object sender, RoutedEventArgs e)
	    {
		    EditorMode = VisualEditorMode.MODE_INSERT_AND_GATE;
	    }

	    private void RemoveComponent(object sender, RoutedEventArgs e)
	    {
		    validSourceElements = new List<Type>() {typeof(FaultTreeAndGateNode), typeof(FaultTreeOrGateNode), typeof(FaultTreeTerminalNode)};
		    EditorMode = VisualEditorMode.MODE_REMOVE_CONTENT;
		    ;
	    }

	    private void AddAndGate(object sender, RoutedEventArgs e)
	    {
		    if (!GraphLayout.Graph.Vertices.Any())
		    {
			    viewModel.FaultTree = new FaultTree.FaultTree(new FaultTreeAndGateNode(0));
		    }
		    else
		    {
			    EditorMode = VisualEditorMode.MODE_ADD_AND_GATE;
			    validSourceElements = new List<Type>() {typeof(FaultTreeOrGateNode), typeof(FaultTreeAndGateNode)};
		    }
	    }

	    private void AddOrGate(object sender, RoutedEventArgs e)
	    {
		    if (!GraphLayout.Graph.Vertices.Any())
		    {
			    viewModel.FaultTree = new FaultTree.FaultTree(new FaultTreeOrGateNode(0));
		    }
		    else
		    {
			    EditorMode = VisualEditorMode.MODE_ADD_OR_GATE;
			    validSourceElements = new List<Type>() {typeof(FaultTreeOrGateNode), typeof(FaultTreeAndGateNode)};
		    }
	    }

	    private void AddBasicEvent(object sender, RoutedEventArgs e)
	    {
		    if (!GraphLayout.Graph.Vertices.Any()) return;

		    EditorMode = VisualEditorMode.MODE_ADD_BASIC_EVENT;
		    validSourceElements = new List<Type>() {typeof(FaultTreeOrGateNode), typeof(FaultTreeAndGateNode)};
	    }

	    private void ChangeRateClick(object sender, MouseButtonEventArgs e)
	    {
		    if (e.ClickCount >= 2)
		    {
			    var t = (Edge<FaultTreeNode>) ((EdgeLabelViewModel) ((FrameworkElement) sender).DataContext).Edge;
			    viewModel.NewEdgeStart = t.Source;
			    viewModel.NewEdgeEnd = t.Destination;
			    viewModel.CreateEdge();
		    }
	    }
    }
}
