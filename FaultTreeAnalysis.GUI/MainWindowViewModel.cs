
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace FaultTreeAnalysis.GUI
{
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

        public string NewEdgeStart { get; set; }

        public string NewEdgeEnd { get; set; }

        public string NewEdgeLabel { get; set; }

        public void CreateEdge()
        {
            if (string.IsNullOrWhiteSpace(this.NewEdgeStart) ||
                string.IsNullOrWhiteSpace(this.NewEdgeEnd))
            {
                return;
            }

            //this.Graph.AddEdge(
            //    new Edge<FaultTreeGate>
            //        (this.GetPerson(this.NewEdgeStart), 
            //        this.GetPerson(this.NewEdgeEnd))
            //        {
            //           Label = this.NewEdgeLabel
            //        });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string property)
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
