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
		public abstract void Write(BinaryDecisionDiagram bdd, FileStream stream);

		public abstract BinaryDecisionDiagram Read(FileStream fileName);

		public virtual BDDTreeFormat GetFormatToken() { return BDDTreeFormat.BDD_TREE_UNKNOWN; }

		public void Write(BinaryDecisionDiagram bdd, String fileName)
		{
			using (var stream = new FileStream(fileName, FileMode.Create))
			{
				Write(bdd, stream);
			}
		}

		public BinaryDecisionDiagram Read(String fileName)
		{
			BinaryDecisionDiagram res;

			//Create stream from file contents
			using (var stream = new FileStream(fileName, FileMode.Open))
			{
				res = Read(stream);
			}

			return res;
		}
	}
}
