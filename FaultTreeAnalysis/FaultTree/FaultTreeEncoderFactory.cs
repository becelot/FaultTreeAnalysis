// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FaultTreeEncoderFactory.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FaultTreeAnalysis.FaultTree
{
    /// <summary>
    /// Creates appropriate codecs based on file extensions
    /// </summary>
    public class FaultTreeEncoderFactory
    {
        /// <summary>
        /// Creates appropriate codecs based on file extensions.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="IFaultTreeCodec"/>.
        /// </returns>
        /// <exception cref="FaultTreeFormatException">
        /// Thrown if file did not contain a valid Fault Tree or was not parsed correctly.
        /// </exception>
        public static IFaultTreeCodec CreateFaultTreeCodec(string fileName)
        {
            if (fileName.EndsWith(".xml"))
            {
                return new XmlFaultTreeEncoder();
            }

            if (fileName.EndsWith(".dot"))
            {
                return new DotFaultTreeEncoder();
            }

            throw new FaultTreeFormatException("The given file was not recognized as a valid format!"); 
        }

        /// <summary>
        /// Creates appropriate codecs based on file extensions.
        /// </summary>
        /// <param name="format">
        /// The codec format.
        /// </param>
        /// <returns>
        /// The <see cref="IFaultTreeCodec"/>.
        /// </returns>
        public static IFaultTreeCodec CreateFaultTreeCodec(FaultTreeFormat format)
        {
            switch (format)
            {
                case FaultTreeFormat.FAULT_TREE_DOT: return new DotFaultTreeEncoder();
                case FaultTreeFormat.FAULT_TREE_XML: return new XmlFaultTreeEncoder();
                default: return null;
            }
        }
    }
}
