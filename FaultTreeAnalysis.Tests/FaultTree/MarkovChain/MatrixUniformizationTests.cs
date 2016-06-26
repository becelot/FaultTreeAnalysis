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
    }
}