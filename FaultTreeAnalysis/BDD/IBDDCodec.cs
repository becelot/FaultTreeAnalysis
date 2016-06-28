// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBDDCodec.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.BDD
{
    using System.IO;

    /// <summary>
    /// The bdd tree format.
    /// </summary>
    public enum BDDTreeFormat
	{
		/// <summary>
		/// Unknown format.
		/// </summary>
		BDD_TREE_UNKNOWN = 0,

		/// <summary>
		/// GraphViz DOT format.
		/// </summary>
		BDD_TREE_DOT,

		/// <summary>
		/// Serializable XML format
		/// </summary>
		BDD_TREE_XML
	}

    /// <summary>
    /// Abstract BDD codec class.
    /// </summary>
    public abstract class IBDDCodec
	{
        /// <summary>
        /// Writes BDD to stream.
        /// </summary>
        /// <param name="bdd">
        /// The bdd.
        /// </param>
        /// <param name="stream">
        /// The stream.
        /// </param>
        public abstract void Write(BinaryDecisionDiagram bdd, FileStream stream);

        /// <summary>
        /// Read BDD from stream.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="BinaryDecisionDiagram"/>.
        /// </returns>
        public abstract BinaryDecisionDiagram Read(FileStream fileName);

        /// <summary>
        /// The format token.
        /// </summary>
        /// <returns>
        /// The <see cref="BDDTreeFormat"/>.
        /// </returns>
        public virtual BDDTreeFormat GetFormatToken()
        {
            return BDDTreeFormat.BDD_TREE_UNKNOWN;
        }

        /// <summary>
        /// Write BDD to file "fileName".
        /// </summary>
        /// <param name="bdd">
        /// The bdd.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        public void Write(BinaryDecisionDiagram bdd, string fileName)
		{
			using (var stream = new FileStream(fileName, FileMode.Create))
			{
			    this.Write(bdd, stream);
			}
		}

        /// <summary>
        /// Read BDD from file "fileName"
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="BinaryDecisionDiagram"/>.
        /// </returns>
        public BinaryDecisionDiagram Read(string fileName)
		{
			BinaryDecisionDiagram res;

            // Create stream from file contents
			using (var stream = new FileStream(fileName, FileMode.Open))
			{
				res = this.Read(stream);
			}

			return res;
		}
	}
}
