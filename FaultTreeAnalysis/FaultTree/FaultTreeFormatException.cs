using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.FaultTree
{
    public class FaultTreeFormatException : Exception
    {
        public FaultTreeFormatException(string message) : base(message)
        {
        }
    }
}
