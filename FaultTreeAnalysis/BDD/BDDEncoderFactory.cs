// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BDDEncoderFactory.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace FaultTreeAnalysis.BDD
{
    /// <summary>
    /// The bdd encoder factory.
    /// </summary>
    public class BDDEncoderFactory
	{
        /// <summary>
        /// Creates a BDD codec from file extension.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="IBDDCodec"/>.
        /// </returns>
        /// <exception cref="BDDFormatException">
        /// Thrown if file does not contain a valid BDD.
        /// </exception>
        public static IBDDCodec CreateFaultTreeCodec(string fileName)
		{
			if (fileName.EndsWith(".dot"))
			{
				return new DotBDDEncoder();
			}

		    if (fileName.EndsWith(".xml"))
		    {
		        return new XmlBDDEncoder();
		    }

		    throw new BDDFormatException("The given file was not recognized as a valid format!");
		}

        /// <summary>
        /// Creates Codec from format token.
        /// </summary>
        /// <param name="format">
        /// The format.
        /// </param>
        /// <returns>
        /// The <see cref="IBDDCodec"/>.
        /// </returns>
        public static IBDDCodec CreateFaultTreeCodec(BDDTreeFormat format)
		{
		    switch (format)
		    {
		        case BDDTreeFormat.BDD_TREE_DOT:
		            return new DotBDDEncoder();
		        case BDDTreeFormat.BDD_TREE_XML:
		            return new XmlBDDEncoder();
		        default:
		            return null;
		    }
		}
	}
}
