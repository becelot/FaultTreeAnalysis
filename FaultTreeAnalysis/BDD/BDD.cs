namespace FaultTreeAnalysis.BDD
{
    public class BDD
    {
        public BDD low;
        public BDD high;

        public int ID;

        public BDD(BDD low, BDD high)
        {
            this.low = low;
            this.high = high;
        }
    }
}
