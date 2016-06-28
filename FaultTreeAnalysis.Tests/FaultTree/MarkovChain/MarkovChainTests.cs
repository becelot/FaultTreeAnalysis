using NUnit.Framework;
using FaultTreeAnalysis.FaultTree.MarkovChain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace FaultTreeAnalysis.Tests.FaultTree.MarkovChain
{
	[TestFixture()]
	public class MarkovChainTests
	{
		[Test()]
		public void MarkovChainTest()
		{
			MarkovChain<int> chains = new MarkovChain<int>(Matrix<double>.Build.Dense(4,4));

			Assert.That(chains.GeneratorMatrix, Is.EqualTo(Matrix<double>.Build.DenseOfArray(new double[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } })));
		}

		[Test()]
		public void MarkovChainTest1()
		{
			MarkovChain<int> chains = new MarkovChain<int>(4);

			Assert.That(chains.GeneratorMatrix, Is.EqualTo(Matrix<double>.Build.DenseOfArray(new double[,] { { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 }, { 0, 0, 0, 0 } })));
		}

		[Test()]
		public void AddEdgeTest()
		{
			MarkovChain<int> chains = new MarkovChain<int>(4);

			chains.AddEdge(0, 3, 0.2);
			chains[7, 10] = 0.7;

			Assert.That(0.2, Is.EqualTo(chains[0,3]));
			Assert.That(0.7, Is.EqualTo(chains[7,10]));
		}

		[Test]
		public void GetAllVerticesTest()
		{
			MarkovChain<int> chains = new MarkovChain<int>(4);

			chains.AddEdge(0, 4, 0.2);
			chains.AddEdge(5, 6, 0.7);
			chains.AddEdge(7, 9, 0.1);

			CollectionAssert.AreEquivalent(new List<int> { 0, 4, 5, 6, 7, 9}, chains.GetAllVertices());
		}

		[Test()]
		public void AddOverflowEdgeTest()
		{
			MarkovChain<int> chains = new MarkovChain<int>(4);

			chains.AddEdge(0, 4, 0.2);
			chains.AddEdge(5, 6, 0.7);
			chains.AddEdge(7, 9, 0.1);

			Assert.That(0.2, Is.EqualTo(chains[0, 4]));
			Assert.That(0.1, Is.EqualTo(chains[7, 9]));
		}

		[Test()]
		public void GetRateTest()
		{
			MarkovChain<int> chains = new MarkovChain<int>(4);

			chains.AddEdge(0, 4, 0.2);
			chains.AddEdge(5, 6, 0.7);
			chains.AddEdge(7, 9, 0.1);

			Assert.That(0.2, Is.EqualTo(chains.GetRate(0,4)));
			Assert.That(0.1, Is.EqualTo(chains.GetRate(7,9)));
			Assert.That(0, Is.EqualTo(chains.GetRate(10, 11)));
		}

		[Test()]
		public void GetOutgoingVerticesTest()
		{
			MarkovChain<int> chains = new MarkovChain<int>(4);

			chains.AddEdge(0, 4, 0.2);
			chains.AddEdge(5, 6, 0.7);
			chains.AddEdge(0, 5, 0.2);

			Assert.That(chains.GetOutgoingVertices(0), Is.EquivalentTo(new List<int> { 4, 5 }));
		}

		[Test()]
		public void GetIncomingVerticesTest()
		{
			MarkovChain<int> chains = new MarkovChain<int>(4);

			chains.AddEdge(0, 4, 0.2);
			chains.AddEdge(5, 6, 0.7);
			chains.AddEdge(0, 5, 0.2);
			chains.AddEdge(4, 5, 0.2);

			Assert.That(chains.GetIncomingVertices(5), Is.EquivalentTo(new List<int> { 0, 4 }));
		}

		[Test()]
		public void GetAllEdgesTest()
		{
			MarkovChain<int> chains = new MarkovChain<int>(7);

			chains.AddEdge(0, 1, 0.2);
			chains.AddEdge(0, 2, 0.7);
			chains.AddEdge(3, 2, 0.2);
			chains.AddEdge(4, 5, 0.2);

			CollectionAssert.AreEquivalent(new List<Tuple<int, double, int>> { new Tuple<int, double, int>(0, 0.2, 1), new Tuple<int, double, int>(0, 0.7, 2), new Tuple<int, double, int>(3, 0.2, 2), new Tuple<int, double, int>(4, 0.2, 5) }, chains.GetAllEdges());
		}

		[Test()]
		public void GetComponentsTest()
		{
			MarkovChain<int> chains = new MarkovChain<int>(7);

			chains.AddEdge(0, 1, 0.2);
			chains.AddEdge(0, 2, 0.7);
			chains.AddEdge(3, 2, 0.2);
			chains.AddEdge(4, 5, 0.2);


			CollectionAssert.AreEquivalent(new List<List<int>> { new List<int> { 0, 1, 2, 3 }, new List<int> { 4, 5 }, new List<int> { 6 } }, chains.GetComponents(new List<int> { 0, 1, 2, 3, 4, 5, 6 }).Select(c => c.OrderBy(v => v).ToList()).ToList());
		}
	}
}