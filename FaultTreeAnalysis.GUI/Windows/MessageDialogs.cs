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

		public static async Task<MessageDialogResult> ShowWarningAsync(string message)
			=> await Core.MainWindow.ShowMessageAsync("Warning", message, MessageDialogStyle.AffirmativeAndNegative);

	    public static async Task<ProgressDialogController> ShowProgressDialog(string title, string message, MetroDialogSettings settings) => 
            await Core.MainWindow.ShowProgressAsync(title, message, false, settings);

	    public static async Task<ProgressDialogController> ShowBDDConstructionDialog() =>
	        await ShowProgressDialog("Binary decision diagram", "Converting Fault Tree to BDD", new MetroDialogSettings());
	}
}
