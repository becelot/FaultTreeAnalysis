// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryDecisionDiagram.cs" company="RWTH-Aachen">
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
    using System.Linq;
    using System.Runtime.Serialization;

    using FaultTreeAnalysis.BDD.BDDTree;
    using FaultTreeAnalysis.FaultTree.Tree;

    /// <summary>
    /// The binary decision diagram.
    /// </summary>
    [DataContract(Name = "BinaryDecisionDiagram")]
	public class BinaryDecisionDiagram
    {
        /// <summary>
        /// Gets or sets the root.
        /// </summary>
        [DataMember]
		public BDDNode Root { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryDecisionDiagram"/> class.
        /// </summary>
        public BinaryDecisionDiagram()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryDecisionDiagram"/> class.
        /// </summary>
        /// <param name="root">
        /// The root.
        /// </param>
        public BinaryDecisionDiagram(BDDNode root)
		{
            this.Root = root;
		}

        /// <summary>
        /// Implicit conversion from <see cref="FaultTreeAnalysis.FaultTree"/>
        /// </summary>
        /// <param name="ft">
        /// The ft.
        /// </param>
        /// <returns>
        /// </returns>
        public static implicit operator BinaryDecisionDiagram(FaultTree.FaultTree ft)
		{
			return new BinaryDecisionDiagram(BDDFactory.GetComponentConnectionInstance().CreateBDD(ft));
		}

        /// <summary>
        /// Flattens the structure to list.
        /// </summary>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public IEnumerable<BDDNode> FlatMap()
		{
			return BDDNode.Traverse(this.Root);
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
            var calculatedNodes = new Dictionary<BDDNode, Tuple<double, double>>();
            return this.Analyze(variableProbability, components, this.Root, new Dictionary<int, bool>(), ref calculatedNodes);
        }

        /// <summary>
        /// The analyze.
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
        /// The calculated Branches. Syntax: (LowBranch, HighBranch)
        /// </param>
        /// <returns>
        /// The <see cref="double"/>.
        /// </returns>
        private double Analyze(
            IEnumerable<Tuple<int, double>> variableProbability,
            IEnumerable<IEnumerable<int>> components,
            BDDNode node,
            Dictionary<int, bool> variableAssignment,
            ref Dictionary<BDDNode, Tuple<double, double>> calculatedBranches)
        {
            if (calculatedBranches.ContainsKey(node))
            {
                var values = calculatedBranches[node];

                var component = components.First(comp => comp.Contains(node.Variable));
                var enumerable = component.Where(variableAssignment.ContainsKey);
                if (enumerable.ToList().FindAll(variable => variableAssignment[variable]).FirstOrDefault() != default(int))
                {
                    return values.Item1;
                }

                if (enumerable.ToList().Count == component.Count() - 1)
                {
                    return values.Item2;
                }

                return values.Item1 + values.Item2;
            }

            return 0;
        }
    }
}
