using System.Windows.Input;

namespace FaultTreeAnalysis.GUI.Windows
{
    public partial class MainWindow
    {
        public static RoutedCommand AddAndGateCommand = new RoutedCommand { InputGestures = { new KeyGesture(Key.A, ModifierKeys.Control)}};
        public static RoutedCommand AddOrGateCommand = new RoutedCommand { InputGestures = { new KeyGesture(Key.O, ModifierKeys.Control) } };
        public static RoutedCommand AddBasicEventCommand = new RoutedCommand { InputGestures = { new KeyGesture(Key.B, ModifierKeys.Control) } };
        public static RoutedCommand AddMarkovChainCommand = new RoutedCommand { InputGestures = { new KeyGesture(Key.M, ModifierKeys.Control) } };
        public static RoutedCommand AddConnectionCommand = new RoutedCommand { InputGestures = { new KeyGesture(Key.C, ModifierKeys.Control) } };
        public static RoutedCommand RemoveComponentCommand = new RoutedCommand { InputGestures = { new KeyGesture(Key.R, ModifierKeys.Control) } };
    }
}
