
using System.Windows;

namespace FaultTreeAnalysis.GUI
{
    public partial class App
    {
	    private void App_OnStartup(object sender, StartupEventArgs e)
	    {
		    Core.Initialize();
	    }
    }
}
