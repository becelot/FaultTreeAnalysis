using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls.Dialogs;

namespace FaultTreeAnalysis.GUI.Windows
{
	public static class MessageDialogs
	{
		public static async Task<string> ShowInputDialogAsync(string title, string body, MetroDialogSettings settings)
			=> await Core.MainWindow.ShowInputAsync(title, body, settings);

		public static async Task<string> ShowRateDialogAsync()
			=> await ShowInputDialogAsync("Rate", "Enter rate for edge", new MetroDialogSettings());
	}
}
