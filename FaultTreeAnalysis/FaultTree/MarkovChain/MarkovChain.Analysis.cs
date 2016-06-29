// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkovChain.Analysis.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FaultTreeAnalysis.FaultTree.MarkovChain
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The markov chain.
    /// </summary>
    public partial class MarkovChain<TVertex>
    {
        /// <summary>
        /// Gets the initial distribution.
        /// </summary>
        [DataMember]
        public Dictionary<TVertex, double> InitialDistribution { get; private set; } = new Dictionary<TVertex, double>();

    }
}
