
namespace FaultTreeAnalysis.GUI
{
    using FaultTree;
    using MahApps.Metro.Controls;
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Util;
    public partial class MainWindow : MetroWindow
	{
		private readonly MainWindowViewModel viewModel;

		public Thickness TitleBarMargin => new Thickness(0, TitlebarHeight, 0, 0);

		public MainWindow()
		{
			viewModel = new MainWindowViewModel();
			DataContext = viewModel;
			InitializeComponent();
			ConsoleManager.Show();
		}

		private void Button_Click(object sender, RoutedEventArgs e) => FlyoutOptions.IsOpen = true;

		private void Example1Open(object sender, RoutedEventArgs e) => LoadFromFile(@"examples\\1341861976041_NO_SEED-ft.dot");
		private void Example2Open(object sender, RoutedEventArgs e) => LoadFromFile(@"examples\\1341861997385_NO_SEED-ft.dot");
		private void Example3Open(object sender, RoutedEventArgs e) => LoadFromFile(@"examples\\1341862261326_NO_SEED-ft.dot");
		private void Example4Open(object sender, RoutedEventArgs e) => LoadFromFile(@"examples\\1341916530531_NO_SEED-ft.dot");
		private void Example5Open(object sender, RoutedEventArgs e) => LoadFromFile(@"examples\\1341916671953_NO_SEED-ft.dot");
		private void Example6Open(object sender, RoutedEventArgs e) => LoadFromFile(@"examples\\1341916816272_NO_SEED-ft.dot");
		private void Example7Open(object sender, RoutedEventArgs e) => LoadFromFile(@"examples\\1341916861497_NO_SEED-ft.dot");
		private void Example8Open(object sender, RoutedEventArgs e) => LoadFromFile(@"examples\\1341916948985_NO_SEED-ft.dot");
		private void Example9Open(object sender, RoutedEventArgs e) => LoadFromFile(@"examples\\1341917031842_NO_SEED-ft.dot");
		private void Example10Open(object sender, RoutedEventArgs e) => LoadFromFile(@"examples\\1341917224042_NO_SEED-ft.dot");

		private void ViewChanged(object sender, RoutedEventArgs e)
		{
			viewModel.FaultTreeView = FaultTreeView.IsChecked.Value;
		}

		private void LoadFromFile(string fileName)
		{
			viewModel.FaultTree = FaultTreeEncoderFactory.CreateFaultTreeCodec(fileName).Read(fileName);
		}

		private void LoadFromFileClick(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog 
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



			// Set filter for file extension and default file extension 
			dlg.DefaultExt = ".dot";
			dlg.Filter = "Dot Files (*.dot)|*.dot|XML Files (*.xml)|*.xml";

			dlg.InitialDirectory = Directory.GetCurrentDirectory();

			// Display OpenFileDialog by calling ShowDialog method 
			Nullable<bool> result = dlg.ShowDialog();


			// Get the selected file name and display in a TextBox 
			if (result == true)
			{
				// Open document 
				string filename = dlg.FileName;
				LoadFromFile(filename);
			}
		}
	}
}
