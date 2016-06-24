using FaultTreeAnalysis.FaultTree.Transformer;

namespace FaultTreeAnalysis.GUI.Util
{
    public static class FaultTreeExtensions
    {
        public static int NextId(this FaultTree.FaultTree ft)
        {
            return ft.Reduce(new FaultTreeMaxIdTransformer()) + 1;
        }

        public static int NextBasicEvent(this FaultTree.FaultTree ft)
        {
            return ft.Reduce(new MaxTerminalTransformer()) + 1;
        }
    }
}
