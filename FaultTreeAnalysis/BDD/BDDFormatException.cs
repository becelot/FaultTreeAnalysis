using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.BDD
{
	public class BDDFormatException : Exception
	{
		public BDDFormatException(string message) : base(message)
        {
		}
	}
}
