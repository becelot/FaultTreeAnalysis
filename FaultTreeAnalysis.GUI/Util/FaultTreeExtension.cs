using FaultTreeAnalysis.FaultTree.Transformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.GUI.Util
{
    public static class FaultTreeExtension
    {
        public static int NextId(this FaultTree.FaultTree ft)
        {
            return ft.reduce<int>(new FaultTreeMaxIdTransformer()) + 1;
        }

        public static int NextBasicEvent(this FaultTree.FaultTree ft)
        {
            return ft.reduce<int>(new MaxTerminalTransformer()) + 1;
        }
    }
}
