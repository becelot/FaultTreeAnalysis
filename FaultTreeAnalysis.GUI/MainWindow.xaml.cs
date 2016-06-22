
namespace FaultTreeAnalysis.GUI
{
	using MahApps.Metro.Controls;
	using System.Windows;

	public partial class MainWindow : MetroWindow
    {
        private MainWindowViewModel viewModel;

		public Thickness TitleBarMargin => new Thickness(0, TitlebarHeight, 0, 0);

		public MainWindow()
        {
            this.viewModel = new MainWindowViewModel();
            this.DataContext = viewModel;
            InitializeComponent();
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
	}
}
