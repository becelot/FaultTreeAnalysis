// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BDDFactory.cs" company="RWTH-Aachen">
//   Benedict Becker, Nico Jansen
// </copyright>
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace FaultTreeAnalysis.BDD
{
    using FaultTreeAnalysis.BDD.BDDTree;

    /// <summary>
    /// The bdd factory.
    /// </summary>
    public abstract class BDDFactory
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        protected static BDDFactory instance = null;

        /// <summary>
        /// The recursive BDD construction singleton instance.
        /// </summary>
        /// <returns>
        /// The <see cref="BDDFactory"/>.
        /// </returns>
        public static BDDFactory GetRecursiveInstance()
        {
			return BDDFactoryRecursive.GetInstance();
        }

        /// <summary>
        /// The component connection BDD construction instance.
        /// </summary>
        /// <returns>
        /// The <see cref="BDDFactory"/>.
        /// </returns>
        public static BDDFactory GetComponentConnectionInstance()
		{
			return BDDFactoryComponentConnection.GetInstance();
		}

        /// <summary>
        /// Creates <see cref="BinaryDecisionDiagram"/> from <see cref="FaultTreeAnalysis.FaultTree"/>
        /// </summary>
        /// <param name="ft">
        /// The ft.
        /// </param>
        /// <returns>
        /// The <see cref="BDDNode"/>.
        /// </returns>
        public abstract BDDNode CreateBDD(FaultTree.FaultTree ft);
    }
}
