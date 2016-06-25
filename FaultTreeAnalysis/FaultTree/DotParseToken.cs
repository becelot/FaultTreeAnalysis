namespace FaultTreeAnalysis.FaultTree
{
    internal enum DotParseToken
    {
        DOT_TRANSITION = 0,
		DOT_MARKOV_TRANSITION = 1,
        DOT_ROOT = 2,
        DOT_GATE = 3,
        DOT_IDENTIFIER = 4,
        DOT_INVALID = 5
    }
}