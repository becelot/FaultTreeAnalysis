using System.ComponentModel;
using FaultTreeAnalysis.FaultTree.Tree;
using FaultTreeAnalysis.GUI.Windows;

namespace FaultTreeAnalysis.GUI.ViewModel
{
	public class DiamondArrow
    {
    }

    public class Arrow
    {        
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
		private bool faultTreeView = true;
		public bool FaultTreeView { get { return faultTreeView; } set { faultTreeView = value; RaisePropertyChanged("FaultTreeView"); RaisePropertyChanged("BDDTreeView"); } }

		public bool BDDTreeView => !faultTreeView;


        private FaultTree.FaultTree faultTree;
		public FaultTree.FaultTree FaultTree
		{
			get { return faultTree; }
			set { faultTree = value; RaisePropertyChanged("FaultTree"); }
		}

        public FaultTreeNode NewEdgeStart { get; set; }

        public FaultTreeNode NewEdgeEnd { get; set; }

        public async void CreateEdge()
        {
            if ((NewEdgeStart == null) ||
                (NewEdgeEnd == null))
            {
                return;
            }

            if (NewEdgeStart == NewEdgeEnd)
            {
                return;
            }

	        if (NewEdgeEnd is FaultTreeTerminalNode && NewEdgeStart is FaultTreeTerminalNode)
	        {
		        string rateString = await MessageDialogs.ShowRateDialogAsync();
		        double rate;
				if (double.TryParse(rateString, out rate))
		        {
					FaultTree.MarkovChain[(FaultTreeTerminalNode) NewEdgeStart, (FaultTreeTerminalNode) NewEdgeEnd] = rate;
				}
		        
	        }
	        else
	        {
				NewEdgeStart.Childs.Add(NewEdgeEnd);
			}
            
            RaisePropertyChanged("FaultTree");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
