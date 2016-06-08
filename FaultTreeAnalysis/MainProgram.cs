using FaultTreeAnalysis.BDD.BDDTree;
using FaultTreeAnalysis.FaultTree;
using FaultTreeAnalysis.FaultTree.Transformer;
using FaultTreeAnalysis.FaultTree.Tree;
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

                ft = ft.deepCopy().replace(1, false).simplify();

                IFaultTreeCodec xmlCodec = FaultTreeEncoderFactory.createFaultTreeCodec(FaultTreeFormat.FAULT_TREE_XML);
                xmlCodec.write(ft, s + ".xml");

                BDD.BDD test = BDDFactory.getInstance().createBDD(ft);
            }

            Console.ReadKey();
        }
    }
}
