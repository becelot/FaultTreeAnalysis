using NUnit.Framework;
using FaultTreeAnalysis.FaultTree.MarkovChain;
using System;
using System.Collections;
using MathNet.Numerics.LinearAlgebra;

namespace FaultTreeAnalysis.Tests.FaultTree.MarkovChain
{
    [TestFixture()]
    public class MatrixUniformizationTests
    {
        public static IEnumerable TestCases
        {
	        get
	        {
		        yield return TestCaseBuilder(0.5, 0.6, 0.2, 1e-10);
				yield return TestCaseBuilder(0.5, 0.6, 35.0d, 1e-6);
				yield return TestCaseBuilder(0.5, 0.6, 0.1, 1e-10);
				yield return TestCaseBuilder(0.5, 0.6, 0.3, 1e-10);
				yield return TestCaseBuilder(0.5, 0.6, 2.0, 1e-10);
				yield return TestCaseBuilder(0.5, 0.6, 20.0, 1e-10);
				yield return TestCaseBuilder(0.4, 0.6, 0.1, 1e-10);
				yield return TestCaseBuilder(0.4, 0.2, 0.1, 1e-10);
				yield return TestCaseBuilder(12.0, 0.6, 0.1, 1e-10);
				yield return TestCaseBuilder(3.4, 0.1, 0.1, 1e-10);
				yield return TestCaseBuilder(3.4, 12.1, 0.7, 1e-10);
			}
        }

        public static IEnumerable TripleChainTestCases
        {
            get
            {
                yield return new TestCaseData(0, Vector<double>.Build.DenseOfArray(new double[] { 0.9, 0.1, 0 }));
                yield return new TestCaseData(1, Vector<double>.Build.DenseOfArray(new double[] { 0.82092, 0.17229, 0.00679 }));
                yield return new TestCaseData(2, Vector<double>.Build.DenseOfArray(new double[] { 0.75246, 0.23090, 0.01664 }));
                yield return new TestCaseData(3, Vector<double>.Build.DenseOfArray(new double[] { 0.69303, 0.27813, 0.02884 }));
                yield return new TestCaseData(4, Vector<double>.Build.DenseOfArray(new double[] { 0.64126, 0.31591, 0.04283 }));
                yield return new TestCaseData(5, Vector<double>.Build.DenseOfArray(new double[] { 0.59602, 0.34583, 0.05815 }));
                yield return new TestCaseData(6, Vector<double>.Build.DenseOfArray(new double[] { 0.55634, 0.36926, 0.07439 }));
            }
        }

        private static TestCaseData TestCaseBuilder(double lambda, double mu, double time, double tolerance)
	    {
			return new TestCaseData(Matrix<double>.Build.DenseOfArray(new[,] { { -lambda, lambda }, { mu, -mu } }), time, tolerance);
		}

		[Test, TestCaseSource(typeof(MatrixUniformizationTests), nameof(TestCases))]
        public void UniformizationTest(Matrix<double> matrix, double time, double tolerance)
        {
	        var goal = matrix.Uniformization(Vector<double>.Build.DenseOfArray(new[] { 1.0d, 0.0d}), time, tolerance);

	        var lambda = matrix[0,1];
	        var mu = matrix[1,0];

	        var test = mu/(lambda + mu) + (lambda/(lambda + mu)*Math.Exp(-(lambda + mu)*time));

			if (!double.IsPositiveInfinity(matrix.ConditionNumber()))
			{
				Assert.That(goal[0], Is.EqualTo(test).Within(tolerance * 100));
			}
        }

        [Test, TestCaseSource(typeof(MatrixUniformizationTests), nameof(TestCases))]
        public void UniformizationTest2(Matrix<double> matrix, double time, double tolerance)
        {
            var ma = matrix.Uniformization(time, tolerance);
            var goal = Vector<double>.Build.DenseOfArray(new[] {1.0d, 0.0d})*ma;

            var lambda = matrix[0, 1];
            var mu = matrix[1, 0];

            var test = mu / (lambda + mu) + (lambda / (lambda + mu) * Math.Exp(-(lambda + mu) * time));

            if (!double.IsPositiveInfinity(matrix.ConditionNumber()))
            {
                Assert.That(goal[0], Is.EqualTo(test).Within(tolerance * 100));
            }
        }

        [Test, TestCaseSource(typeof(MatrixUniformizationTests), nameof(TripleChainTestCases))]
        public void TripleTest(int n, Vector<double> vector)
        {
            Matrix<double> matrix = Matrix<double>.Build.DenseOfArray(new double[,]
            {
                { -0.2, 0.1, 0},
                { 0.2, -0.2, 0.05},
                { 0, 0.1, -0.05}
            }).Transpose();


            Vector<double> initialDistribution = Vector<double>.Build.DenseOfArray(new double[] { 0.9, 0.1, 0 });
            var m = matrix.Uniformization(0.5, 1e-16);
            var p = initialDistribution;
            for (int i = 0; i < n; i++)
            {
                p = p*m;
            }

            Assert.That(p[0], Is.EqualTo(vector[0]).Within(1e-5));
            Assert.That(p[1], Is.EqualTo(vector[1]).Within(1e-5));
            Assert.That(p[2], Is.EqualTo(vector[2]).Within(1e-5));
        }
    }
}