﻿namespace FaultTreeAnalysis.GUI.FlyoutControls
{
	/// <summary>
	/// Interaction logic for OptionsMain.xaml
	/// </summary>
	public partial class OptionsMain
	{
		public OptionsMain()
		{
			this.DataContext = Config.Instance;
			this.InitializeComponent();
		}
	}
}
