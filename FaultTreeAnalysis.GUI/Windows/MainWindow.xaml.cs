
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using FaultTreeAnalysis.FaultTree;
using FaultTreeAnalysis.GUI.Util;
using FaultTreeAnalysis.GUI.ViewModel;

namespace FaultTreeAnalysis.GUI.Windows
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using FaultTreeAnalysis.FaultTree.Tree;

    public partial class MainWindow
    {
		public readonly MainWindowViewModel ViewModel;

		public Thickness TitleBarMargin => new Thickness(0, this.TitlebarHeight / 2, 0, 0);

        public int FlyoutHeight => (int) (this.Height*0.75);

        public MainWindow()
		{
            ConsoleManager.Show();
			Config.Load();
			this.ViewModel = new MainWindowViewModel();
		    this.DataContext = this.ViewModel;
		    this.InitializeComponent();
			
		}

		private void Button_Click(object sender, RoutedEventArgs e) => this.FlyoutOptions.IsOpen = true;

		private void Example1Open(object sender, RoutedEventArgs e) => this.LoadFromFile(@"examples\\1341861976041_NO_SEED-ft.dot");
		private void Example2Open(object sender, RoutedEventArgs e) => this.LoadFromFile(@"examples\\1341861997385_NO_SEED-ft.dot");
		private void Example3Open(object sender, RoutedEventArgs e) => this.LoadFromFile(@"examples\\1341862261326_NO_SEED-ft.dot");
		private void Example4Open(object sender, RoutedEventArgs e) => this.LoadFromFile(@"examples\\1341916530531_NO_SEED-ft.dot");
		private void Example5Open(object sender, RoutedEventArgs e) => this.LoadFromFile(@"examples\\1341916671953_NO_SEED-ft.dot");
		private void Example6Open(object sender, RoutedEventArgs e) => this.LoadFromFile(@"examples\\1341916816272_NO_SEED-ft.dot");
		private void Example7Open(object sender, RoutedEventArgs e) => this.LoadFromFile(@"examples\\1341916861497_NO_SEED-ft.dot");
		private void Example8Open(object sender, RoutedEventArgs e) => this.LoadFromFile(@"examples\\1341916948985_NO_SEED-ft.dot");
		private void Example9Open(object sender, RoutedEventArgs e) => this.LoadFromFile(@"examples\\1341917031842_NO_SEED-ft.dot");
		private void Example10Open(object sender, RoutedEventArgs e) => this.LoadFromFile(@"examples\\1341917224042_NO_SEED-ft.dot");
	    private void TestCaseOpen(object sender, RoutedEventArgs e) => this.LoadFromFile(@"examples\\TestCase.xml");

		private void ViewChanged(object sender, RoutedEventArgs e)
		{
		    this.ViewModel.FaultTreeView = this.FaultTreeView.IsChecked.GetValueOrDefault();
		}

		private void LoadFromFile(string fileName)
		{
		    this.ViewModel.FaultTree = FaultTreeEncoderFactory.CreateFaultTreeCodec(fileName).Read(fileName);
		}

		private void LoadFromFileClick(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog 
		    Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
		    {
		        DefaultExt = ".dot",
		        Filter = "Dot Files (*.dot)|*.dot|XML Files (*.xml)|*.xml",
		        InitialDirectory = Directory.GetCurrentDirectory()
		    };



		    // Set filter for file extension and default file extension 


		    // Display OpenFileDialog by calling ShowDialog method 
			bool? result = dlg.ShowDialog();


			// Get the selected file name and display in a TextBox 
			if (result == true)
			{
				// Open document 
				string filename = dlg.FileName;
			    this.LoadFromFile(filename);
			}
		}

	    private void SaveToFile(string fileName)
	    {
		    FaultTreeEncoderFactory.CreateFaultTreeCodec(FaultTreeFormat.FAULT_TREE_XML).Write(this.ViewModel.FaultTree, fileName);
	    }

		private void SaveProjectClick(object sender, RoutedEventArgs e)
		{
			// Create OpenFileDialog 
			Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog()
			{
				DefaultExt = ".xml",
				Filter = "XML Files (*.xml)|*.xml",
				InitialDirectory = Directory.GetCurrentDirectory()
			};

			// Set filter for file extension and default file extension 


			// Display OpenFileDialog by calling ShowDialog method 
			bool? result = dlg.ShowDialog();


			// Get the selected file name and display in a TextBox 
			if (result == true)
			{
				// Open document 
				string filename = dlg.FileName;
			    this.SaveToFile(filename);
			}
		}
    }
}
