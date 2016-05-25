using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FaultTreeAnalysis
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            List<String> files = new List<String>(Directory.GetFiles("examples")).Where(f => new Regex(@".*-ft.*").IsMatch(f)).ToList();

            Console.ReadKey();
        }
    }
}
