﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MarkovChain.Analysis.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FaultTreeAnalysis.FaultTree.MarkovChain
{
    using System.CodeDom;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Runtime.Serialization;

    using MathNet.Numerics.LinearAlgebra;

    /// <summary>
    /// The markov chain.
    /// </summary>
    public partial class MarkovChain<TVertex>
    {
        /// <summary>
        /// Gets the initial distribution.
        /// </summary>
        [DataMember]
        public Dictionary<TVertex, double> InitialDistribution { get; set; } = new Dictionary<TVertex, double>();

	    internal void NormalizeDistribution()
	    {
		    var components = this.GetComponents(entryMap.Keys);
		    foreach (var component in components)
		    {
			    var componentList = component.ToList();
			    double sum = componentList.Select(vertex => InitialDistribution[vertex]).Sum();
			    if (sum == 0)
			    {
				    double val = 1.0d/componentList.Count();
					componentList.ForEach(vertex => InitialDistribution[vertex] = val);
			    }
			    else
			    {
				    componentList.ForEach(vertex => InitialDistribution[vertex] /= sum);
			    }
		    }
	    }

#pragma warning disable 1591
        public Dictionary<TVertex, List<double>> ComputeProbability(double samplingRate, double timeSpan, double errorTolerance)
#pragma warning restore 1591
        {
            Dictionary<TVertex, List<double>> result = new Dictionary<TVertex, List<double>>();
            this.entryMap.Keys.ToList().ForEach(vertex => result.Add(vertex, new List<double>()));

            // Components of system
            var components = this.GetComponents(this.entryMap.Keys);
            int stepNum = (int)(timeSpan / samplingRate);
            // Connected component
            foreach (var component in components)
            {
                
                // Construct Generator matrix
                var componentList = component.ToList();
                if (componentList.Count == 1)
                {
                    result[componentList[0]] =
                        Enumerable.Repeat(this.InitialDistribution[componentList[0]], stepNum+1).ToList();
                }
                else
                {
                    Matrix<double> genMatrix = Matrix<double>.Build.Dense(componentList.Count(), componentList.Count(), (i, j) => this[componentList[i], componentList[j]]);
                    genMatrix -= Matrix<double>.Build.DenseIdentity(componentList.Count(), componentList.Count())
                                 * Matrix<double>.Build.Diagonal(genMatrix.RowSums().ToArray());

                    var uniformMatrix = genMatrix.Uniformization(samplingRate, errorTolerance);
                    Vector<double> currentProbability = Vector<double>.Build.Dense(
                        componentList.Count(),
                        i => this.InitialDistribution[componentList[i]]);

                    for (int i = 0; i <= stepNum; i++)
                    {
                        for (int j = 0; j < componentList.Count(); j++)
                        {
                            result[componentList[j]].Add(currentProbability[j]);
                        }
                        currentProbability *= uniformMatrix;
                    }
                }

            }

            return result;
        }

    }
}
