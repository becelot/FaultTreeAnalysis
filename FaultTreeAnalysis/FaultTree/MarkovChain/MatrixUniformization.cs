using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

	        Matrix<double> Qprime = matrix/q + Matrix<double>.Build.DenseIdentity(2,2);

			var sum = 0.0d;
	        int l = 1;
			for (l = 1; true; l++)
	        {
				sum += Math.Pow(q * time, l-1) / Factorial(l-1);
				if (Math.Exp(-q*time)*sum <= errorTolerance/2)
				{
			        continue;
		        }
		        break;
	        }
	        l--;

	        int k = 1;
	        if (l > 0)
	        {
		        sum = 1;
		        for (k = 1; true; k++)
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
				for (k = 1; true; k++)
				{
					sum += Math.Pow(q * time, k) / Factorial(k);
					if ((1 - Math.Exp(-q * time) * sum) > errorTolerance)
					{
						continue;
					}
					break;
				}
			}

	        Vector<double> S = Vector<double>.Build.Dense(matrix.RowCount);
	        Vector<double> Pl = initialDistributon*Qprime.Power(l);
	        Vector<double> R = Pl*Math.Pow(q*time, l)/Factorial(l);

	        for (int i = l+1; i <= k; i++)
	        {
		        S = S + R;
		        R = R*Qprime*q*time/ Math.Max(i, 1);
	        }

            return (S * Math.Exp(-q * time));
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
