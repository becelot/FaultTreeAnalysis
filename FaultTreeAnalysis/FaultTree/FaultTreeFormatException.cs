using System;

namespace FaultTreeAnalysis.FaultTree
{
    public class FaultTreeFormatException : Exception
    {
        public FaultTreeFormatException(String message) : base(message)
        {
        }
    }
}
