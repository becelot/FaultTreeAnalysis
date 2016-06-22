using FaultTreeAnalysis.BDD.BDDTree;
using FaultTreeAnalysis.FaultTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace FaultTreeAnalysis
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            List<String> files = new List<String>(Directory.GetFiles("examples")).Where(f => new Regex(@".*-ft.dot$").IsMatch(f)).ToList();

            foreach(String s in files)
            {
				Console.WriteLine("Converting tree " + s);

                IFaultTreeCodec codec = FaultTreeEncoderFactory.createFaultTreeCodec(s);
                FaultTree.FaultTree ft = codec.read(s);

                //ft = ft.deepCopy().replace(1, false).simplify();

                //IFaultTreeCodec xmlCodec = FaultTreeEncoderFactory.createFaultTreeCodec(FaultTreeFormat.FAULT_TREE_XML);
                //xmlCodec.write(ft, s + ".xml");

                BDDNode test = BDDFactory.getComponentConnectionInstance().createBDD(ft);
                Console.WriteLine("Successfully converted tree");

				using (StreamWriter fs = new StreamWriter(s + ".bdd.dot", false, System.Text.Encoding.UTF8, 65536))
				{
					test.WriteToFile(fs);
					//fs.Write(test.ToString());
				}
            }
            Console.WriteLine("Finished construction");
            Console.ReadKey();
        }
    }
}
