using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaultTreeAnalysis.GUI.Windows;

namespace FaultTreeAnalysis.GUI
{
	public static class Core
	{
		public static MainWindow MainWindow { get; set; }

		public static void Initialize()
		{
			MainWindow = new MainWindow();
			MainWindow.Show();
		}
	}
}
