namespace FaultTreeAnalysis.BDD
{
	public class BDDEncoderFactory
	{
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

		public static IBDDCodec CreateFaultTreeCodec(BDDTreeFormat format)
		{
			switch (format)
			{
				case BDDTreeFormat.BDD_TREE_DOT: return new DotBDDEncoder();
				case BDDTreeFormat.BDD_TREE_XML: return new XmlBDDEncoder();
				default: return null;
			}
		}
	}
}
