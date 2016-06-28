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
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using FaultTreeAnalysis.BDD.BDDTree;

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
	}
}
