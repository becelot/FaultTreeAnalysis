using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.BDD
{
	public class BDDEncoderFactory
	{
		public static IBDDCodec createFaultTreeCodec(string fileName)
		{
			if (fileName.EndsWith(".dot"))
			{
				return new DotBDDEncoder();
			}

			throw new BDDFormatException("The given file was not recognized as a valid format!");
		}

		public static IBDDCodec createFaultTreeCodec(BDDTreeFormat format)
		{
			switch (format)
			{
				case BDDTreeFormat.BDD_TREE_DOT: return new DotBDDEncoder();
				default: return null;
			}
		}
	}
}
