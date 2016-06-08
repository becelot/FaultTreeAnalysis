using FaultTreeAnalysis.FaultTree;
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
            List<String> files = new List<String>(Directory.GetFiles("examples")).Where(f => new Regex(@".*-ft.dot").IsMatch(f)).ToList();

            foreach(String s in files.Take(1))
            {
                IFaultTreeCodec codec = FaultTreeEncoderFactory.createFaultTreeCodec(s);
                FaultTree.FaultTree ft = codec.read(s);

                //ft = ft.reduce(new ReplaceTransformer(1, false));

                IFaultTreeCodec xmlCodec = FaultTreeEncoderFactory.createFaultTreeCodec(FaultTreeFormat.FAULT_TREE_XML);
                xmlCodec.write(ft, s + ".xml");
            }

            Console.ReadKey();
        }
    }
}
