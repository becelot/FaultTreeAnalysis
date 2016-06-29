// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkovChain.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using System.Runtime.Serialization;

namespace FaultTreeAnalysis.FaultTree.MarkovChain
{
    /// <summary>
    /// Markov chain representation class
    /// </summary>
    /// <typeparam name="TVertex"></typeparam>
    [DataContract(Name = "MarkovChain")]
    public partial class MarkovChain<TVertex>
    {
        private Matrix<double> RateMatrix { get; set; }

        /// <summary>
        /// The generator matrix (deprecated).
        /// </summary>
        public Matrix<double> GeneratorMatrix => this.RateMatrix;

        private Dictionary<TVertex, int> entryMap;

        /// <summary>
        /// Gets or sets the MarkovChain property. Used for serialization of MarkovChain.
        /// </summary>
        [DataMember]
        private IEnumerable<Tuple<TVertex, double, TVertex>> MarkovArray
        {
            get
            {
                return this.GetAllEdges();
            }

            set
            {
                int row = value.Select(v => v.Item1).Union(value.Select(v => v.Item3)).GroupBy(v => v).Count();

                // MarkovChain = new MarkovChain<FaultTreeTerminalNode>(Matrix<double>.Build.DenseOfIndexed(row, row, value));
                this.RateMatrix = Matrix<double>.Build.Dense(row, row);
                this.entryMap = new Dictionary<TVertex, int>();
                foreach (var trans in value)
                {
                    this[trans.Item1, trans.Item3] = trans.Item2;
                }
            }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="MarkovChain{TVertex}"/> class.
        /// </summary>
        /// <param name="rateMatrix"></param>
        public MarkovChain(Matrix<double> rateMatrix)
        {
            this.RateMatrix = rateMatrix;
            this.entryMap = new Dictionary<TVertex, int>();
        }

        private TVertex GetVertexFromIndex(int index)
        {
            return this.entryMap.Keys.FirstOrDefault(k => this.entryMap[k] == index);
        }

        private int GetIndexOfVertex(TVertex vertex)
        {
            if (this.entryMap.ContainsKey(vertex))
            {
                return this.entryMap[vertex];
            }

            int max = this.entryMap.Values.DefaultIfEmpty(-1).Max();

            if (max + 2 > this.RateMatrix.ColumnCount)
            {
                this.RateMatrix = Matrix<double>.Build.Dense(max + 2, max + 2, (i, j) => i == max+1 || j == max+1 ? 0 : this.RateMatrix[i,j]);
            }

            this.entryMap.Add(vertex, max+1);

            return max + 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkovChain{TVertex}"/> class.
        /// </summary>
        /// <param name="size">Initial size of Markov chain</param>
        public MarkovChain(int size)
        {
            this.RateMatrix = Matrix<double>.Build.Dense(size, size);
            this.entryMap = new Dictionary<TVertex, int>();
        }

        /// <summary>
        /// Maps two Vertices of the Markov chain to the transition rate.
        /// </summary>
        /// <param name="from">Source vertex.</param>
        /// <param name="to">Destination vertex.</param>
        public double this[TVertex from, TVertex to] {
            get { return this.GetRate(from, to); }
            set {
                this.AddEdge(from, to, value); }
        }

        private double this[int i, int j] => this.RateMatrix[i,j];

        /// <summary>
        /// Adds an edge to the markov chain from sourceVertex to destinationVertex with transition rate. 
        /// If the source or destination vertex do not exist, the chain size is automatically adjusted 
        /// to fit the new entry.
        /// </summary>
        /// <param name="sourceVertex">The source vertex.</param>
        /// <param name="destinationVertex">The destination vertex.</param>
        /// <param name="rate">The transition rate.</param>
        public void AddEdge(TVertex sourceVertex, TVertex destinationVertex, double rate)
        {
	        int rowIndex = this.GetIndexOfVertex(sourceVertex);
	        int colIndex = this.GetIndexOfVertex(destinationVertex);
            this.RateMatrix[rowIndex, colIndex] = rate;
        }


	    /// <summary>
	    /// Retrieves the rate of edge sourceVertex to destinationVertex.
	    /// </summary>
	    /// <param name="sourceVertex">The source vertex.</param>
	    /// <param name="destinationVertex">The destination vertex.</param>
	    /// <returns>The transition rate. Returns 0 if no edge exists or source/destinationVertex are not contained in this Markov chain collection.</returns>
	    public double GetRate(TVertex sourceVertex, TVertex destinationVertex)
        {
			int rowIndex = this.GetIndexOfVertex(sourceVertex);
			int colIndex = this.GetIndexOfVertex(destinationVertex);
			return this.RateMatrix[rowIndex, colIndex];
        }

        /// <summary>
        /// Returns all outgoing vertices from vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>List of vertices. Returns emptylist if vertex is not contained in chain.</returns>
        public IEnumerable<TVertex> GetOutgoingVertices(TVertex vertex)
        {
            int index = this.GetIndexOfVertex(vertex);
            for (int i = 0; i < this.RateMatrix.RowCount; i++)
            {
                if (this.RateMatrix[index, i] != 0)
                {
                    yield return this.GetVertexFromIndex(i);
                }
            }
        }

        /// <summary>
        /// Returns all incoming vertices in vertex.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        /// <returns>List of vertices. Returns emptylist if vertex is not contained in chain.</returns>
        public IEnumerable<TVertex> GetIncomingVertices(TVertex vertex)
        {
            int index = this.GetIndexOfVertex(vertex);
            for (int i = 0; i < this.RateMatrix.RowCount; i++)
            {
                if (this.RateMatrix[i, index] != 0)
                {
                    yield return this.GetVertexFromIndex(i);
                }
            }
        }

        /// <summary>
        /// Retrieve all added vertices.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TVertex> GetAllVertices()
        {
            foreach (var vertex in this.entryMap.Keys)
            {
                yield return vertex;
            }
        }

        /// <summary>
        /// Retrieve all edges in this Markov chain.
        /// </summary>
        /// <returns>List of edges as tuples. Format: (sourceVertex, transition rate, destination vertex).</returns>
        public IEnumerable<Tuple<TVertex, double, TVertex>> GetAllEdges()
        {
            for (int i = 0; i < this.RateMatrix.ColumnCount; i++)
            {
                for (int j = 0; j < this.RateMatrix.ColumnCount; j++)
                {
                    if (this[i, j] != 0)
                    {
                        yield return new Tuple<TVertex, double, TVertex>(this.GetVertexFromIndex(i), this[i,j], this.GetVertexFromIndex(j));
                    }
                }
            }
        }

        /// <summary>
        /// Returns a list of all connected markov compoennts.
        /// </summary>
        /// <param name="vertices">Available vertices. Note that this parameter is required since the Markov Chain does not necessarily know all vertices in the system. Those vertices are assigned a connected component with one element.</param>
        /// <returns></returns>
        public IEnumerable<IEnumerable<TVertex>> GetComponents(IEnumerable<TVertex> vertices)
        {
            HashSet<TVertex> visited = new HashSet<TVertex>();

            foreach (var vertex in vertices)
            {
                this.GetIndexOfVertex(vertex);
                if (visited.Contains(vertex)) continue;

                HashSet<TVertex> component = new HashSet<TVertex>();
                Stack<TVertex> toVisit = new Stack<TVertex>();
                toVisit.Push(vertex);

                while (toVisit.Any())
                {
                    var current = toVisit.Pop();
                    if (visited.Contains(current)) continue;

                    component.Add(current);
                    visited.Add(current);

                    this.GetOutgoingVertices(current).ToList().ForEach(v => toVisit.Push(v));
                    this.GetIncomingVertices(current).ToList().ForEach(v => toVisit.Push(v));
                }

                yield return component;
            }
        }
    }
}
