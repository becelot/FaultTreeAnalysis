// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFaultTreeCodec.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FaultTreeAnalysis.FaultTree
{
    using System.Diagnostics.CodeAnalysis;
    using System.IO;

    /// <summary>
    /// The fault tree format.
    /// </summary>
    public enum FaultTreeFormat
    {
        /// <summary>
        /// Format not known.
        /// </summary>
        FAULT_TREE_UNKNOWN = 0,
        /// <summary>
        /// FraphViz dot file.
        /// </summary>
        FAULT_TREE_DOT,
        /// <summary>
        /// Serialized XML file.
        /// </summary>
        FAULT_TREE_XML
    }

    /// <summary>
    /// Abstract class meant to be implemented as an interface.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public abstract class IFaultTreeCodec
    {
        /// <summary>
        /// Write FaultTree into stream.
        /// </summary>
        /// <param name="ft">
        /// The FaultTree.
        /// </param>
        /// <param name="stream">
        /// The stream the Fault Tree is written to.
        /// </param>
        public abstract void Write(FaultTree ft, FileStream stream);

        /// <summary>
        /// Reads a Fault Tree from a given stream.
        /// </summary>
        /// <param name="fileName">
        /// The stream containing the Fault Tree.
        /// </param>
        /// <returns>
        /// The <see cref="FaultTree"/>.
        /// </returns>
        public abstract FaultTree Read(FileStream fileName);

        /// <summary>
        /// Get the recognized format of the current codec.
        /// </summary>
        /// <returns>Format</returns>
        public virtual FaultTreeFormat GetFormatToken() => FaultTreeFormat.FAULT_TREE_UNKNOWN;

        /// <summary>
        /// Write FaultTree into file.
        /// </summary>
        /// <param name="ft">The Fault Tree</param>
        /// <param name="fileName">The file to be written.</param>
        public void Write(FaultTree ft, string fileName)
        {
            using (var stream = new FileStream(fileName, FileMode.Create))
            {
                this.Write(ft, stream);
            }
        }

        /// <summary>
        /// Reads a Fault Tree from a given file.
        /// </summary>
        /// <param name="fileName">Path of the file.</param>
        /// <returns>The Fault Tree</returns>
        public FaultTree Read(string fileName)
        {
            FaultTree res;

            //Create stream from file contents
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                res = this.Read(stream);
            }

            return res;
        }
    }
}
