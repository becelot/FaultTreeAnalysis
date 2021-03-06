﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryDecisionDiagram.Analyze.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FaultTreeAnalysis.BDD
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using BDDTree;
    using FaultTree.Tree;

    /// <summary>
    /// The binary decision diagram.
    /// </summary>
    public partial class BinaryDecisionDiagram
    {
        /// <summary>
        /// Get the actual probability of node.
        /// </summary>
        /// <param name="components">
        /// The components.
        /// </param>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="variableAssignment">
        /// The variable assignment.
        /// </param>
        /// <param name="calculatedBranches">
        /// The already calculated nodes.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>double</cref>
        ///     </see>
        ///     .
        /// </returns>
        private static double GetProbability(
            IEnumerable<IEnumerable<int>> components,
            BDDNode node,
            Dictionary<int, bool> variableAssignment,
            Dictionary<BDDNode, Tuple<double, double>> calculatedBranches)
        {
            var values = calculatedBranches[node];

            var component = components.First(comp => comp.Contains(node.Variable)).ToList();
            var alreadyVisitedVariables = component.Where(variableAssignment.ContainsKey).ToList();
            if (alreadyVisitedVariables.ToList().FindAll(variable => variableAssignment[variable]).FirstOrDefault()
                != default(int))
            {
                return values.Item1;
            }

            if (alreadyVisitedVariables.ToList().Count == component.Count() - 1)
            {
                return values.Item2;
            }

            return values.Item1 + values.Item2;
        }

        /// <summary>
        /// Analyze the BDD given a variable time series.
        /// </summary>
        /// <param name="analyzeTime">
        /// Map of variable to time series of probabilities.
        /// </param>
        /// <param name="components">
        /// The components.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        internal IEnumerable<double> Analyze(Dictionary<FaultTreeTerminalNode, List<double>> analyzeTime, IEnumerable<IEnumerable<FaultTreeTerminalNode>> components)
        {
            var steps = analyzeTime.Values.First().Count;
            for (int i = 0; i < steps; i++)
            {
                var currentMapping = analyzeTime.Keys.Select(terminal => new Tuple<int, double>(terminal.Label, analyzeTime[terminal][i]));
                // ReSharper disable once PossibleMultipleEnumeration
                yield return this.Analyze(currentMapping, components.Select(component => component.Select(node => node.Label)));
            }
        }

        /// <summary>
        /// Analyze the BDD given a variable time series.
        /// </summary>
        /// <param name="variableProbability">
        /// The variable probability.
        /// </param>
        /// <param name="components">
        /// The components.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        private double Analyze(
            IEnumerable<Tuple<int, double>> variableProbability,
            IEnumerable<IEnumerable<int>> components)
        {
            var calculatedNodes = new Dictionary<BDDNode, double>();
            return this.Analyze(variableProbability, components, this.Root, new Dictionary<int, bool>(), ref calculatedNodes);
        }

        /// <summary>
        /// Analyze the BDD given a variable probability distribution.
        /// </summary>
        /// <param name="variableProbability">
        /// The variable probability.
        /// </param>
        /// <param name="components">
        /// The components.
        /// </param>
        /// <param name="node">
        /// The node.
        /// </param>
        /// <param name="variableAssignment">
        /// The variable assignment.
        /// </param>
        /// <param name="calculatedBranches">
        /// The already calculated nodes. Syntax: (LowBranch, HighBranch)
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        private double Analyze(
            IEnumerable<Tuple<int, double>> variableProbability,
            IEnumerable<IEnumerable<int>> components,
            BDDNode node,
            Dictionary<int, bool> variableAssignment,
            ref Dictionary<BDDNode, double> calculatedBranches)
        {
            if (node is BDDTerminalNode)
            {
                return ((BDDTerminalNode)node).Value ? 1 : 0;
            }

            if (calculatedBranches.ContainsKey(node))
            {
                return calculatedBranches[node];
            }

            var lowDict = variableAssignment.ToDictionary(entry => entry.Key, entry => entry.Value);

            double low = this.Analyze(
                variableProbability,
                components,
                node.LowNode,
                lowDict,
                ref calculatedBranches);

            var highDict = variableAssignment.ToDictionary(entry => entry.Key, entry => entry.Value);
            highDict.Add(node.Variable, true);

            var component = components.First(comp => comp.Contains(node.Variable)).ToList();
            var nextDisjoint = node.HighNode;
            while (component.Contains(nextDisjoint.Variable))
            {
                nextDisjoint = nextDisjoint.LowNode;
            }

            double high = this.Analyze(
                variableProbability,
                components,
                nextDisjoint,
                highDict,
                ref calculatedBranches);

            double pG;
            
            if (!component.Contains(node.LowNode.Variable))
            {
                pG = low + (variableProbability.ToList().Find(tup => tup.Item1 == node.Variable).Item2 * (high - low));
            }
            else
            {
                var i2 = node.LowNode;
                while (component.Contains(i2.Variable))
                {
                    i2 = i2.LowNode;
                }

                pG = low + (variableProbability.ToList().Find(tup => tup.Item1 == node.Variable).Item2 * (high - this.Analyze(variableProbability, components, i2, highDict, ref calculatedBranches)));
            }

            calculatedBranches[node] = pG;
            return pG;
        }
    }
}
