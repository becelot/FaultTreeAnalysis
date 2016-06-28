// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultTreeFormatException.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FaultTreeAnalysis.FaultTree
{
    using System;

    /// <summary>
    /// Thrown if file could not be parsed into Fault Tree.
    /// </summary>
    public class FaultTreeFormatException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FaultTreeFormatException"/> class. 
        /// </summary>
        /// <param name="message">
        /// Message to be thrown.
        /// </param>
        public FaultTreeFormatException(string message) : base(message)
        {
        }
    }
}
