using NUnit.Framework;
using FaultTreeAnalysis.FaultTree.MarkovChain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Single;

namespace FaultTreeAnalysis.Tests.FaultTree.MarkovChain
{
    [TestFixture()]
    public class MatrixUniformizationTests
    {
        public static IEnumerable TestCases
        {
	        get
	        {
		        yield return TestCaseBuilder(0.5, 0.6, 0.2, 1e-16);
				yield return TestCaseBuilder(0.5, 0.6, 0.1, 1e-16);
				yield return TestCaseBuilder(0.5, 0.6, 0.3, 1e-16);
				yield return TestCaseBuilder(0.5, 0.6, 2.0, 1e-16);
				yield return TestCaseBuilder(0.5, 0.6, 20.0, 1e-16);
			}
        }

	    private static TestCaseData TestCaseBuilder(double lambda, double mu, double time, double tolerance)
	    {
			return new TestCaseData(Matrix<double>.Build.DenseOfArray(new double[,] { { -lambda, lambda }, { mu, -mu } }), time, tolerance).Returns(true);
		}

		[Test, TestCaseSource(typeof(MatrixUniformizationTests), "TestCases")]
        public bool UniformizationTest(Matrix<double> matrix, double time, double tolerance)
        {
	        var goal = matrix.Uniformization(Vector<double>.Build.DenseOfArray(new double[] { 1.0d, 0.0d}), time, tolerance);

	        var lambda = matrix[1,0];
	        var mu = matrix[1,0];

	        var test = mu/(lambda + mu) + (lambda/(lambda + mu)*Math.Exp(-(lambda + mu)*time));

			return test - goal[0] < tolerance*10;
        }
    }

    public class UniformizationTestCases
    {
        
    }
}