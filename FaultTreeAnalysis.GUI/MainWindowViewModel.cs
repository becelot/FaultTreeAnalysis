
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace FaultTreeAnalysis.GUI
{
    using FaultTree.Tree;
    using System.ComponentModel;

    public class DiamondArrow
    {
    }

    public class Arrow
    {        
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
		private bool faultTreeView = true;
		public bool FaultTreeView { get { return faultTreeView; } set { faultTreeView = value; this.RaisePropertyChanged("FaultTreeView"); this.RaisePropertyChanged("BDDTreeView"); } }

		public bool BDDTreeView { get { return !faultTreeView; } }


		private FaultTree.FaultTree faultTree;
		public FaultTree.FaultTree FaultTree
		{
			get { return faultTree; }
			set { faultTree = value; this.RaisePropertyChanged("FaultTree"); }
		}

        public MainWindowViewModel()
        {
        }

        /*public IEnumerable<string> PersonNames
        {
            get { return this.Graph.AllVertices.Select(x => x.Name); }
        }*/

        public FaultTreeNode NewEdgeStart { get; set; }

        public FaultTreeNode NewEdgeEnd { get; set; }

        public string NewEdgeLabel { get; set; }

        public void CreateEdge()
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

            this.NewEdgeStart.Childs.Add(this.NewEdgeEnd);
            this.RaisePropertyChanged("FaultTree");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        //private FaultTreeGate GetPerson(string name)
        //{
        //    return this.Graph.AllVertices.First(x => string.CompareOrdinal(x.Name, name) == 0);
        //}
    }
}
