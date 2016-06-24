using System;

namespace FaultTreeAnalysis.BDD
{
	public class BDDFormatException : Exception
	{
		public BDDFormatException(string message) : base(message)
        {
		}
	}
}
