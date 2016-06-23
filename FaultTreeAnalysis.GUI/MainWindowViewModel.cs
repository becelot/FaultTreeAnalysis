﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace FaultTreeAnalysis.GUI
{
    using Graphviz4Net.Graphs;
    using System.ComponentModel;

    public class Person : INotifyPropertyChanged
    {
        private readonly Graph<Person> graph;

        public Person(Graph<Person> graph)
        {
            this.graph = graph;
            this.Avatar = "./Avatars/avatarAnon.gif";
        }

    	private string name;
    	public string Name
    	{
    		get { return this.name; }
    		set
    		{
    			this.name = value;
    			if (this.PropertyChanged != null) {
    				this.PropertyChanged(this, new PropertyChangedEventArgs("Name"));
    			}
    		}
    	}

    	public string Avatar { get; set; }

        public string Email
        {
            get
            {
                return this.Name.ToLower().Replace(' ', '.') + "@gmail.com";
            }
        }

        public ICommand RemoveCommand
        {
            get { return new RemoveCommandImpl(this); }
        }

        private class RemoveCommandImpl : ICommand
        {
            private Person person;

            public RemoveCommandImpl(Person person)
            {
                this.person = person;
            }

            public void Execute(object parameter)
            {
                this.person.graph.RemoveVertexWithEdges(this.person);
            }

            public bool CanExecute(object parameter)
            {
                return true;
            }

            public event EventHandler CanExecuteChanged;
        }

    	public event PropertyChangedEventHandler PropertyChanged;
    }

	public class FaultTreeGate
	{

		public FaultTreeGate()
		{

		}
		public string Name { get; set; }
	}

	public class FaultTreeAndGate : FaultTreeGate
	{
		public FaultTreeAndGate() : base() { }
	}

	public class FaultTreeOrGate : FaultTreeGate
	{
		public FaultTreeOrGate() : base() { }
	}

    public class DiamondArrow
    {
    }

    public class Arrow
    {        
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
		private bool faultTreeView = true;
		public bool FaultTreeView { get { return faultTreeView; } set { faultTreeView = value; this.RaisePropertyChanged("FaultTreeView"); } }

		private bool bddTreeView = true;
		public bool BDDTreeView { get { return bddTreeView; } set { bddTreeView = value; this.RaisePropertyChanged("BDDTreeView"); } }


		private FaultTree.FaultTree faultTree;
		public FaultTree.FaultTree FaultTree
		{
			get { return faultTree; }
			set { faultTree = value; this.RaisePropertyChanged("FaultTree"); }
		}

        public MainWindowViewModel()
        {
            var graph = new Graph<FaultTreeGate>();
			var a = new FaultTreeAndGate() { Name = "Jonh" };
			var b = new FaultTreeOrGate() { Name = "Jonh" };
			var c = new FaultTreeAndGate() { Name = "Jonh"};

			graph.AddVertex(a);
			graph.AddVertex(b);
			graph.AddVertex(c);

			graph.AddEdge(new Edge<FaultTreeGate>(a, b));

			/*var a = new Person(graph) { Name = "Jonh", Avatar = "./Avatars/avatar1.jpg" };
            var b = new Person(graph) { Name = "Michael", Avatar = "./Avatars/avatar2.gif" };
            var c = new Person(graph) { Name = "Kenny" };
            var d = new Person(graph) { Name = "Lisa" };
            var e = new Person(graph) { Name = "Lucy", Avatar = "./Avatars/avatar3.jpg" };
            var f = new Person(graph) { Name = "Ted Mosby" };
            var g = new Person(graph) { Name = "Glen" };
            var h = new Person(graph) { Name = "Alice", Avatar = "./Avatars/avatar1.jpg" };

            graph.AddVertex(a);
            graph.AddVertex(b);
            graph.AddVertex(c);
            graph.AddVertex(d);
            graph.AddVertex(e);
            graph.AddVertex(f);

            var subGraph = new SubGraph<Person> { Label = "Work" };
            graph.AddSubGraph(subGraph);
            subGraph.AddVertex(g);
            subGraph.AddVertex(h);
            graph.AddEdge(new Edge<Person>(g, h));
            graph.AddEdge(new Edge<Person>(a, g));

            var subGraph2 = new SubGraph<Person> {Label = "School"};
            graph.AddSubGraph(subGraph2);
            var loner = new Person(graph) { Name = "Loner", Avatar = "./Avatars/avatar1.jpg" };
            subGraph2.AddVertex(loner);
            graph.AddEdge(new Edge<SubGraph<Person>>(subGraph, subGraph2) { Label = "Link between groups" } );

            graph.AddEdge(new Edge<Person>(c, d) { Label = "In love", DestinationArrowLabel = "boyfriend", SourceArrowLabel = "girlfriend" });

            graph.AddEdge(new Edge<Person>(c, g, new Arrow(), new Arrow()));
            graph.AddEdge(new Edge<Person>(c, a, new Arrow()) { Label = "Boss" });
            graph.AddEdge(new Edge<Person>(d, h, new DiamondArrow(), new DiamondArrow()));
            graph.AddEdge(new Edge<Person>(f, h, new DiamondArrow(), new DiamondArrow()));
            graph.AddEdge(new Edge<Person>(f, loner, new DiamondArrow(), new DiamondArrow()));
            graph.AddEdge(new Edge<Person>(f, b, new DiamondArrow(), new DiamondArrow()));
            graph.AddEdge(new Edge<Person>(e, g, new Arrow(), new Arrow()) { Label = "Siblings" });*/

			this.Graph = graph;
            this.Graph.Changed += GraphChanged;
            this.NewPersonName = "Enter new name";
        	this.UpdatePersonNewName = "Enter new name";
        }

        public Graph<FaultTreeGate> Graph { get; private set; }

        public string NewPersonName { get; set; }

		public string UpdatePersonName { get; set; }

		public string UpdatePersonNewName { get; set; }

        public IEnumerable<string> PersonNames
        {
            get { return this.Graph.AllVertices.Select(x => x.Name); }
        }

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

            this.Graph.AddEdge(
                new Edge<FaultTreeGate>
                    (this.GetPerson(this.NewEdgeStart), 
                    this.GetPerson(this.NewEdgeEnd))
                    {
                        Label = this.NewEdgeLabel
                    });
        }

        public void CreatePerson()
        {
            if (this.PersonNames.Any(x => x == this.NewPersonName))
            {
                // such a person already exists: there should be some validation message, but 
                // it is not so important in a demo
                return;
            }

            var p = new FaultTreeGate() { Name = this.NewPersonName };
            this.Graph.AddVertex(p);
        }

		public void UpdatePerson()
		{
			if (string.IsNullOrWhiteSpace(this.UpdatePersonName)) 
			{
				return;
			}

			this.GetPerson(this.UpdatePersonName).Name = this.UpdatePersonNewName;
			this.RaisePropertyChanged("PersonNames");
			this.RaisePropertyChanged("Graph");
		}

        public event PropertyChangedEventHandler PropertyChanged;

        private void GraphChanged(object sender, GraphChangedArgs e)
        {
            this.RaisePropertyChanged("PersonNames");
        }

        private void RaisePropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private FaultTreeGate GetPerson(string name)
        {
            return this.Graph.AllVertices.First(x => string.CompareOrdinal(x.Name, name) == 0);
        }
    }
}
