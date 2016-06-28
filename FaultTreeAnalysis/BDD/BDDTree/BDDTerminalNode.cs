// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BDDTerminalNode.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.BDD.BDDTree
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The bdd terminal node.
    /// </summary>
    [DataContract(Name = "BDDTerminalNode")]
    public class BDDTerminalNode : BDDNode
    {
        /// <summary>
        /// Gets or sets a value indicating whether value.
        /// </summary>
        [DataMember]
		public bool Value { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BDDTerminalNode"/> class.
        /// </summary>
        public BDDTerminalNode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BDDTerminalNode"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public BDDTerminalNode(bool value)
        {
            this.Value = value;
        }
	}
}
