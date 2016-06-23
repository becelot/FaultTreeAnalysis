
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
			//this.AddNewEdge.Click += AddNewEdgeClick;
			//this.AddNewPerson.Click += AddNewPersonClick;
			//this.UpdatePerson.Click += UpdatePersonClick;
		}

		void UpdatePersonClick(object sender, RoutedEventArgs e)
		{
			//this.viewModel.UpdatePersonName = (string) this.UpdatePersonName.SelectedItem;
			this.viewModel.UpdatePerson();
		}

		private void AddNewPersonClick(object sender, RoutedEventArgs e)
		{
			this.viewModel.CreatePerson();
		}

		private void AddNewEdgeClick(object sender, RoutedEventArgs e)
		{
			//this.viewModel.NewEdgeStart = (string) this.NewEdgeStart.SelectedItem;
			//this.viewModel.NewEdgeEnd = (string)this.NewEdgeEnd.SelectedItem;
			this.viewModel.CreateEdge();
		}

		private void Button_Click(object sender, RoutedEventArgs e) => FlyoutOptions.IsOpen = true;

		private void ViewChanged(object sender, RoutedEventArgs e)
		{
			viewModel.FaultTreeView = FaultTreeView.IsChecked.Value;
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
				viewModel.FaultTree = FaultTreeEncoderFactory.createFaultTreeCodec(filename).read(filename);
				Console.WriteLine("created");
			}
		}
	}
}
