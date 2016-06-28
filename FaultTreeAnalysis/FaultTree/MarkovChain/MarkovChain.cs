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

namespace FaultTreeAnalysis.FaultTree.MarkovChain
{
    public class MarkovChain<TVertex>
    {
        private Matrix<double> RateMatrix { get; set; }

        public Matrix<double> GeneratorMatrix => RateMatrix;

        private readonly Dictionary<TVertex, int> entryMap;

        public MarkovChain(Matrix<double> rateMatrix)
        {
            RateMatrix = rateMatrix;
            entryMap = new Dictionary<TVertex, int>();
        }

        private TVertex GetVertexFromIndex(int index)
        {
            return entryMap.Keys.FirstOrDefault(k => entryMap[k] == index);
        }

        private int GetIndexOfVertex(TVertex vertex)
        {
            if (entryMap.ContainsKey(vertex))
            {
                return entryMap[vertex];
            }

            int max = entryMap.Values.DefaultIfEmpty(-1).Max();

            if (max + 2 > RateMatrix.ColumnCount)
            {
                RateMatrix = Matrix<double>.Build.Dense(max + 2, max + 2, (i, j) => i == max+1 || j == max+1 ? 0 : RateMatrix[i,j]);
            } 

            entryMap.Add(vertex, max+1);

            return max + 1;
        }

        public MarkovChain(int size)
        {
            RateMatrix = Matrix<double>.Build.Dense(size, size);
            entryMap = new Dictionary<TVertex, int>();
        }

        public double this[TVertex from, TVertex to] {
            get { return GetRate(from, to); }
            set { AddEdge(from, to, value); }
        }

        private double this[int i, int j] => RateMatrix[i,j];

        public void AddEdge(TVertex sourceVertex, TVertex destinationVertex, double rate)
        {
	        int rowIndex = GetIndexOfVertex(sourceVertex);
	        int colIndex = GetIndexOfVertex(destinationVertex);
	        RateMatrix[rowIndex, colIndex] = rate;
        }

	    public double GetRate(TVertex sourceVertex, TVertex destinationVertex)
        {
			int rowIndex = GetIndexOfVertex(sourceVertex);
			int colIndex = GetIndexOfVertex(destinationVertex);
			return RateMatrix[rowIndex, colIndex];
        }

        public IEnumerable<TVertex> GetOutgoingVertices(TVertex vertex)
        {
            int index = GetIndexOfVertex(vertex);
            for (int i = 0; i < RateMatrix.RowCount; i++)
            {
                if (RateMatrix[index, i] != 0)
                {
                    yield return GetVertexFromIndex(i);
                }
            }
        }

        public IEnumerable<TVertex> GetIncomingVertices(TVertex vertex)
        {
            int index = GetIndexOfVertex(vertex);
            for (int i = 0; i < RateMatrix.RowCount; i++)
            {
                if (RateMatrix[i, index] != 0)
                {
                    yield return GetVertexFromIndex(i);
                }
            }
        }

        public IEnumerable<TVertex> GetAllVertices()
        {
            foreach (var vertex in entryMap.Keys)
            {
                yield return vertex;
            }
        }

        public IEnumerable<Tuple<TVertex, double, TVertex>> GetAllEdges()
        {
            for (int i = 0; i < RateMatrix.ColumnCount; i++)
            {
                for (int j = 0; j < RateMatrix.ColumnCount; j++)
                {
                    if (this[i, j] != 0)
                    {
                        yield return new Tuple<TVertex, double, TVertex>(GetVertexFromIndex(i), this[i,j], GetVertexFromIndex(j));
                    }
                }
            }
        }

        public IEnumerable<IEnumerable<TVertex>> GetComponents(IEnumerable<TVertex> vertices)
        {
            HashSet<TVertex> visited = new HashSet<TVertex>();

            foreach (var vertex in vertices)
            {
	            GetIndexOfVertex(vertex);
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

                    GetOutgoingVertices(current).ToList().ForEach(v => toVisit.Push(v));
                    GetIncomingVertices(current).ToList().ForEach(v => toVisit.Push(v));
                }

                yield return component;
            }
        }
    }
}
