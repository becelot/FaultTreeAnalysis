using System;
using MathNet.Numerics.LinearAlgebra;

namespace FaultTreeAnalysis.FaultTree.MarkovChain
{
    public static class MatrixUniformization
    {
        public static Vector<double> Uniformization(this Matrix<double> matrix, Vector<double> initialDistributon,  double time, double errorTolerance)
        {
			
	        double q = double.MinValue;
	        for (int i = 0; i < matrix.RowCount; i++)
	        {
		        for (int j = 0; j < matrix.ColumnCount; j++)
		        {
			        if (Math.Abs( matrix[i, j]) > q)
			        {
				        q = Math.Abs( matrix[i, j] );
			        }
		        }
	        }

	        Matrix<double> qprime = matrix/q + Matrix<double>.Build.DenseIdentity(2,2);

			var sum = 0.0d;
	        int l;
			for (l = 1;; l++)
	        {
				sum += Math.Pow(q * time, l-1) / Factorial(l-1);
				if (Math.Exp(-q*time)*sum <= errorTolerance/2)
				{
			        continue;
		        }
		        break;
	        }
	        l--;

	        int k;
	        if (l > 0)
	        {
		        sum = 1;
		        for (k = 1;; k++)
		        {
			        sum += Math.Pow(q*time, k)/Factorial(k);
			        if ((1 - Math.Exp(-q*time)*sum) > errorTolerance/2)
			        {
				        continue;
			        }
			        break;
		        }
	        }
	        else
	        {
				sum = 1;
				for (k = 1;; k++)
				{
					sum += Math.Pow(q * time, k) / Factorial(k);
					if ((1 - Math.Exp(-q * time) * sum) > errorTolerance)
					{
						continue;
					}
					break;
				}
			}

	        Vector<double> s = Vector<double>.Build.Dense(matrix.RowCount);
	        Vector<double> pl = initialDistributon*qprime.Power(l);
	        Vector<double> r = pl*Math.Pow(q*time, l)/Factorial(l);

	        for (int i = l+1; i <= k; i++)
	        {
		        s = s + r;
		        r = r*qprime*q*time/ Math.Max(i, 1);
	        }

            return (s * Math.Exp(-q * time));
        }

	    public static long Factorial(int l)
	    {
		    long res = 1L;

		    for (int i = 1; i <= l; i++)
		    {
			    res *= i;
		    }

		    return res;
	    }
    }
}
