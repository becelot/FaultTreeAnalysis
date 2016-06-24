using System;
using System.IO;

namespace FaultTreeAnalysis.BDD
{
	public enum BDDTreeFormat
	{
		BDD_TREE_UNKNOWN = 0,
		BDD_TREE_DOT,
		BDD_TREE_XML
	}


	public abstract class IBDDCodec
	{
		public abstract void write(BinaryDecisionDiagram bdd, FileStream stream);

		public abstract BinaryDecisionDiagram read(FileStream fileName);

		public virtual BDDTreeFormat getFormatToken() { return BDDTreeFormat.BDD_TREE_UNKNOWN; }

		public void write(BinaryDecisionDiagram bdd, String fileName)
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Create))
			{
				write(bdd, stream);
			}
		}

		public BinaryDecisionDiagram read(String fileName)
		{
			BinaryDecisionDiagram res;

			//Create stream from file contents
			using (FileStream stream = new FileStream(fileName, FileMode.Open))
			{
				res = read(stream);
			}

			return res;
		}
	}
}
