using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FaultTreeAnalysis.GUI
{
    public partial class MainWindow
    {
        private void FaultTreeZoomControl_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var canvas = FaultTreeGridView;

            var canvasLine = canvas.Children.OfType<Line>().LastOrDefault();
            if (canvasLine == null)
            {
                var startPoint = e.GetPosition(canvas);
                var line = new Line
                {
                    Stroke = Brushes.Blue,
                    StrokeThickness = 3,
                    X1 = startPoint.X,
                    Y1 = startPoint.Y,
                    X2 = startPoint.X,
                    Y2 = startPoint.Y,
                };

                canvas.Children.Add(line);
            }
            else
            {
                canvas.Children.Remove(canvasLine);
            }
        }

        private void FaultTreeZoomControl_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
        }

        private void FaultTreeZoomControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

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
    }
}
