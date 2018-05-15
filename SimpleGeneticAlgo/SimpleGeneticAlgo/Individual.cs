using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticString
{
    abstract class Individual
    {
        public static Random RndMutateur { get; set; }

        static Individual()
        {
            RndMutateur = new Random();
        }

        public Individual() { }
        /// <summary>
        /// Retourne deux enfants ayant ensemble les genes des deux parents
        /// </summary>
        /// <param name="partner">Partenaire avec lequel l'individu partagera ses genes</param>
        /// <returns>Tableau des deux enfants creer</returns>
        public abstract Individual[] ReturnChildrens(Individual partner);

        public abstract string GiveInformations();



    }
}
