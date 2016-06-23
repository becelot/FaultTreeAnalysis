
namespace FaultTreeAnalysis.GUI
{
	using FaultTree;
	using Util;
	using MahApps.Metro.Controls;
	using System;
	using System.IO;
	using System.Windows;
	using WinForms = System.Windows.Forms;

	public partial class MainWindow : MetroWindow
	{
		private MainWindowViewModel viewModel;

		public Thickness TitleBarMargin => new Thickness(0, TitlebarHeight, 0, 0);

		public MainWindow()
		{
			this.viewModel = new MainWindowViewModel();
			this.DataContext = viewModel;
			InitializeComponent();
			ConsoleManager.Show();
		}

		private void Button_Click(object sender, RoutedEventArgs e) => FlyoutOptions.IsOpen = true;

		private void Example1Open(object sender, RoutedEventArgs e) => this.LoadFromFile(@"examples\\1341861976041_NO_SEED-ft.dot");

		private void ViewChanged(object sender, RoutedEventArgs e)
		{
			viewModel.FaultTreeView = FaultTreeView.IsChecked.Value;
		}

		private void LoadFromFile(string fileName)
		{
			viewModel.FaultTree = FaultTreeEncoderFactory.createFaultTreeCodec(fileName).read(fileName);
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
				this.LoadFromFile(filename);
			}
		}
	}
}
