using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.GUI.Converters
{
    using System.Globalization;
    using System.Windows.Data;

    using FaultTreeAnalysis.FaultTree.Tree;

    public class TerminalNodeToDistributionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "Initial distribution: " + Core.MainWindow.ViewModel.FaultTree.MarkovChain.InitialDistribution[(FaultTreeTerminalNode)value];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
