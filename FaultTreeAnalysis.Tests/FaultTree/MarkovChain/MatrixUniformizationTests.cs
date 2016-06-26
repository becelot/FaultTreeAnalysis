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
                yield return new TestCaseData(Matrix<double>.Build.DenseOfArray(new double[,] { {-0.5, 0.6}, {0.5, 0.6} })).Returns(0);
            }
        }

        [Test, TestCaseSource(typeof(MatrixUniformizationTests), "TestCases")]
        public double UniformizationTest(Matrix<double> matrix)
        {
            return 0;
        }
    }

    public class UniformizationTestCases
    {
        
    }
}