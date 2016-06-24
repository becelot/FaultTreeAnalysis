using FaultTreeAnalysis.FaultTree.Transformer;
using FaultTreeAnalysis.FaultTree.Tree;
using FaultTreeAnalysis.GUI.Util;
using Graphviz4Net.Graphs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FaultTreeAnalysis.GUI
{
    public partial class MainWindow
    {
        private List<Type> validSourceElements = new List<Type>() {  };
        private List<Type> validDestinationElements = new List<Type>() {  };

        public enum VisualEditorMode
        {
            MODE_ADD_AND_GATE,
            MODE_ADD_OR_GATE,
            MODE_ADD_BASIC_EVENT,
            MODE_ADD_GATE_CONNECTION,
            MODE_ADD_MARKOV_CHAIN
        }

        public VisualEditorMode EditorMode { get; private set; }

        private void EditorDownGate(Grid sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FaultTreeNode vertex;

            switch(EditorMode)
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
        }

        private void EditorDownConnection(Grid sender, System.Windows.Input.MouseButtonEventArgs e)
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

                this.viewModel.NewEdgeStart = (FaultTreeNode)sender.DataContext;
            }
            else
            {
                if (!validDestinationElements.Contains(sender.DataContext.GetType()))
                {
                    return;
                }
                canvas.Children.Remove(canvasLine);

                this.viewModel.NewEdgeEnd = (FaultTreeNode)sender.DataContext;
                this.viewModel.CreateEdge();
            }
        }

        private void FaultTreeZoomControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            switch (EditorMode)
            {
                case VisualEditorMode.MODE_ADD_GATE_CONNECTION:
                    EditorDownConnection((Grid)sender, e);
                    break;
                case VisualEditorMode.MODE_ADD_AND_GATE:
                case VisualEditorMode.MODE_ADD_OR_GATE:
                case VisualEditorMode.MODE_ADD_BASIC_EVENT:
                    EditorDownGate((Grid)sender, e);
                    break;
                default: break;
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
            if (line == null)
            {
                return;
            }
            if (line != null)
            {

                var endPoint = e.GetPosition(canvas);
                var directed = endPoint - new Point(line.X1, line.Y1);
                directed.Normalize();
                line.X2 = endPoint.X - directed.X * 2;
                line.Y2 = endPoint.Y - directed.Y * 2;
            }
        }

        private void AddGateConnection(object sender, RoutedEventArgs e)
        {
            this.validSourceElements = new List<Type>() { typeof(FaultTreeAndGateNode), typeof(FaultTreeOrGateNode) };
            this.validDestinationElements = new List<Type>() { typeof(FaultTreeAndGateNode), typeof(FaultTreeOrGateNode), typeof(FaultTreeTerminalNode) };
            this.EditorMode = VisualEditorMode.MODE_ADD_GATE_CONNECTION;
        }

        private void AddMarkovChain(object sender, RoutedEventArgs e)
        {
            this.validSourceElements = new List<Type>() { typeof(FaultTreeTerminalNode) };
            this.validDestinationElements = new List<Type>() { typeof(FaultTreeTerminalNode) };
            this.EditorMode = VisualEditorMode.MODE_ADD_MARKOV_CHAIN;
        }

        private void AddAndGate(object sender, RoutedEventArgs e)
        {
            if (this.GraphLayout.Graph.Vertices.Count() == 0)
            {
                this.viewModel.FaultTree = new FaultTree.FaultTree(new FaultTreeAndGateNode(0));
            } else
            {
                EditorMode = VisualEditorMode.MODE_ADD_AND_GATE;
                this.validSourceElements = new List<Type>() { typeof(FaultTreeOrGateNode), typeof(FaultTreeAndGateNode) };
            }
        }

        private void AddOrGate(object sender, RoutedEventArgs e)
        {
            if (this.GraphLayout.Graph.Vertices.Count() == 0)
            {
                this.viewModel.FaultTree = new FaultTree.FaultTree(new FaultTreeOrGateNode(0));
            }
            else
            {
                EditorMode = VisualEditorMode.MODE_ADD_OR_GATE;
                this.validSourceElements = new List<Type>() { typeof(FaultTreeOrGateNode), typeof(FaultTreeAndGateNode) };
            }
        }

        private void AddBasicEvent(object sender, RoutedEventArgs e)
        {
            if (this.GraphLayout.Graph.Vertices.Count() == 0)
            {
                return;
            } else
            {
                EditorMode = VisualEditorMode.MODE_ADD_BASIC_EVENT;
                this.validSourceElements = new List<Type>() { typeof(FaultTreeOrGateNode), typeof(FaultTreeAndGateNode) };
            }
        }
    }
}
