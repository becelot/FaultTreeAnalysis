// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultTree.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.FaultTree
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using FaultTreeAnalysis.BDD;

    using global::FaultTreeAnalysis.FaultTree.MarkovChain;
    using Transformer;
    using Tree;

    /// <summary>
    /// Class representing a Fault Tree, containing both the tree and markov chains
    /// </summary>
    [DataContract(Name = "FaultTree")]
    public class FaultTree
    {
        /// <summary>
        /// Gets or sets node of FaultTree
        /// </summary>
        [DataMember]
        public FaultTreeNode Root { get; set; }

        /// <summary>
        /// Gets or sets all Markov chains found in FaultTree
        /// </summary>
        [DataMember]
		public MarkovChain<FaultTreeTerminalNode> MarkovChain { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTree"/> class.
        /// </summary>
        public FaultTree()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTree"/> class. 
        /// </summary>
        /// <param name="root">
        /// Root node of FaultTree
        /// </param>
        public FaultTree(FaultTreeNode root)
        {
            this.Root = root;
            this.MarkovChain = new MarkovChain<FaultTreeTerminalNode>(1);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTree"/> class. 
        /// </summary>
        /// <param name="root">
        /// Root node of FaultTree
        /// </param>
        /// <param name="markovChain">
        /// Markov chain used in <see cref="FaultTree"/>
        /// </param>
	    public FaultTree(FaultTreeNode root, MarkovChain<FaultTreeTerminalNode> markovChain)
	    {
            this.MarkovChain = markovChain;
            this.Root = root;
	    }

        /// <summary>
        /// Reduce the FaultTree using a transformer to a T object
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="tr">TreeTransformer used for conversion</param>
        /// <returns>Reduced T</returns>
        public T Reduce<T>(FaultTreeTransformer<T> tr)
        {
            return this.Root.Reduce(tr);
        }

        /// <summary>
        /// FaultTree wrapper for Reduce that produces a new FaultTree based on the used transformer.
        /// </summary>
        /// <param name="tr">The Tree Transformer</param>
        /// <returns>The <see cref="FaultTree"/></returns>
        public FaultTree TreeMap(FaultTreeTransformer<FaultTreeNode> tr)
        {
            return new FaultTree(this.Root.Reduce(tr));
        }

        /// <summary>
        /// Wrapper method that executes DeepCopy Transformer on FaultTree
        /// </summary>
        /// <returns>The <see cref="FaultTree"/></returns>
        public FaultTree DeepCopy()
        {
            return this.TreeMap(new DeepCopyTransformer());
        }

        /// <summary>
        /// Wrapper method that executes Replace Transformer on FaultTree
        /// </summary>
        /// <param name="label">Label of <see cref="FaultTreeTerminalNode"/></param>
        /// <param name="value">Value that <see cref="FaultTreeTerminalNode"/> is replaced with</param>
        /// <returns>The <see cref="FaultTree"/></returns>
        public FaultTree Replace(int label, bool value)
        {
            return this.TreeMap(new ReplaceTransformer(label, value));
        }

        /// <summary>
        /// Wrapper method that executes Simplify Transformer on FaultTree
        /// </summary>
        /// <returns>The <see cref="FaultTree"/></returns>
        public FaultTree Simplify()
        {
            return this.TreeMap(new SimplifyTransformer());
        }

        /// <summary>
        /// Flattens tree structure to list of nodes that occur in the tree
        /// </summary>
        /// <returns>List of <see cref="FaultTreeNode"/></returns>
		public IEnumerable<FaultTreeNode> Traverse()
		{
			Stack<FaultTreeNode> stack = new Stack<FaultTreeNode>();

			HashSet<FaultTreeNode> visited = new HashSet<FaultTreeNode>();
			stack.Push(this.Root);

			while (stack.Count > 0)
			{
				var current = stack.Pop();

				if (visited.Contains(current))
				{
					continue;
				}

				visited.Add(current);
				yield return current;

			    var node = current as FaultTreeGateNode;
			    if (node == null)
			    {
			        continue;
			    }

			    foreach (var n in node.Childs)
			    {
			        stack.Push(n);
			    }
			}

		    foreach (var node in this.MarkovChain.GetAllVertices())
		    {
		        if (!visited.Contains(node))
		        {
		            yield return node;
		        }
		    }
		}

        /// <summary>
        /// The analyze.
        /// </summary>
        /// <param name="samplingRate">
        /// The sampling rate.
        /// </param>
        /// <param name="timeSpan">
        /// The time span.
        /// </param>
        /// <param name="errorTolerance">
        /// The error tolerance.
        /// </param>
        /// <returns>
        /// The <see>
        ///         <cref>IEnumerable</cref>
        ///     </see>
        ///     .
        /// </returns>
        public IEnumerable<double> Analyze(double samplingRate, double timeSpan, double errorTolerance)
        {
            BinaryDecisionDiagram bdd = this;
            return bdd.Analyze(this.MarkovChain.ComputeProbability(samplingRate, timeSpan, errorTolerance), this.MarkovChain.GetComponents((List<FaultTreeTerminalNode>)this.Traverse()));
        }
	}
}
