using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            if (max + 1 > RateMatrix.ColumnCount)
            {
                RateMatrix = Matrix<double>.Build.Dense(max + 1, max + 1, (i, j) => i == max+1 || j == max+1 ? 0 : RateMatrix[i,j]);
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
            RateMatrix[GetIndexOfVertex(sourceVertex), GetIndexOfVertex(destinationVertex)] = rate;
        }

        public double GetRate(TVertex sourceVertex, TVertex destinationVertex)
        {
            return RateMatrix[GetIndexOfVertex(sourceVertex), GetIndexOfVertex(destinationVertex)];
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
    }
}
