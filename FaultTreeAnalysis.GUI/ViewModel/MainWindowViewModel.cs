using System.ComponentModel;
using FaultTreeAnalysis.FaultTree.Tree;
using FaultTreeAnalysis.GUI.Windows;

namespace FaultTreeAnalysis.GUI.ViewModel
{
    using System.Runtime.CompilerServices;

    using FaultTreeAnalysis.GUI.Annotations;

    public class DiamondArrow
    {
    }

    public class Arrow
    {        
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
		private bool faultTreeView = true;
		public bool FaultTreeView {
		    get
		    {
		        return this.faultTreeView;
		    }

            set
            {
		        this.faultTreeView = value;
		        this.RaisePropertyChanged();
		        this.RaisePropertyChanged("BDDTreeView");
            }
        }

		public bool BDDTreeView => !this.faultTreeView;


        private FaultTree.FaultTree faultTree;
		public FaultTree.FaultTree FaultTree
		{
			get { return this.faultTree; }
			set {
			    this.faultTree = value;
			    this.RaisePropertyChanged(); }
		}

        public FaultTreeNode NewEdgeStart { get; set; }

        public FaultTreeNode NewEdgeEnd { get; set; }

        public async void CreateEdge()
        {
            if ((this.NewEdgeStart == null) ||
                (this.NewEdgeEnd == null))
            {
                return;
            }

            if (this.NewEdgeStart == this.NewEdgeEnd)
            {
                return;
            }

	        if (this.NewEdgeEnd is FaultTreeTerminalNode && this.NewEdgeStart is FaultTreeTerminalNode)
	        {
		        string rateString = await MessageDialogs.ShowRateDialogAsync();
		        double rate;
				if (double.TryParse(rateString, out rate))
		        {
		            this.FaultTree.MarkovChain[(FaultTreeTerminalNode)this.NewEdgeStart, (FaultTreeTerminalNode)this.NewEdgeEnd] = rate;
				}
		        
	        }
	        else
	        {
	            this.NewEdgeStart.Childs.Add(this.NewEdgeEnd);
			}

            this.RaisePropertyChanged("FaultTree");
        }

        private int flyoutHeight;
        public int FlyoutHeight
        {
            get { return this.flyoutHeight; }
            set
            {
                if (value.Equals(this.flyoutHeight)) return;
                this.flyoutHeight = value;
                this.RaisePropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        public void RaisePropertyChanged([CallerMemberName] string property = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
