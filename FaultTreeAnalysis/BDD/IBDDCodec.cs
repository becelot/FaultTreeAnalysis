using FaultTreeAnalysis.BDD.BDDTree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaultTreeAnalysis.BDD
{
	public enum BDDTreeFormat
	{
		BDD_TREE_UNKNOWN = 0,
		BDD_TREE_DOT
	};


	public abstract class IBDDCodec
	{
		public abstract void write(BDD bdd, FileStream stream);

		public abstract BDD read(FileStream fileName);

		public virtual BDDTreeFormat getFormatToken() { return BDDTreeFormat.BDD_TREE_UNKNOWN; }

		public void write(BDD bdd, String fileName)
		{
			using (FileStream stream = new FileStream(fileName, FileMode.Create))
			{
				this.write(bdd, stream);
			}
		}

		public BDD read(String fileName)
		{
			BDD res;

			//Create stream from file contents
			using (FileStream stream = new FileStream(fileName, FileMode.Open))
			{
				res = this.read(stream);
			}

			return res;
		}
	}
}
