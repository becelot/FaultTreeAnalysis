// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BDDFormatException.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.BDD
{
    using System;

    /// <summary>
    /// The bdd format exception thrown when the file is not a valid BDD.
    /// </summary>
    public class BDDFormatException : Exception
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="BDDFormatException"/> class.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public BDDFormatException(string message) : base(message)
        {
		}
	}
}
