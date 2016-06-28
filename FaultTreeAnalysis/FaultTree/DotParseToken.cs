// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DotParseToken.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FaultTreeAnalysis.FaultTree
{
    /// <summary>
    /// The dot parse token.
    /// </summary>
    internal enum DotParseToken
    {
        DOT_TRANSITION = 0,
		DOT_MARKOV_TRANSITION = 1,
        DOT_ROOT = 2,
        DOT_GATE = 3,
        DOT_IDENTIFIER = 4,
        DOT_IMPLICIT_CHAIN = 5,
        DOT_INVALID = 6
    }
}