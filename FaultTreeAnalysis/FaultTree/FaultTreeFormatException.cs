using System;

namespace FaultTreeAnalysis.FaultTree
{
    public class FaultTreeFormatException : Exception
    {
        public FaultTreeFormatException(string message) : base(message)
        {
        }
    }
}
