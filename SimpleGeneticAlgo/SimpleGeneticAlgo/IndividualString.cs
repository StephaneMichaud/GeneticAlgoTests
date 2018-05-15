using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGeneticAlgo
{
    class IndividualString:Individual
    {
        //Constantes
        /// <summary>
        /// Pourcentage de mutation sur chaque element du gene code
        /// </summary>
        const float RANDOM_CHANCE = 1;
        /// <summary>
        /// Charactere possible pouvant former le gene code
        /// </summary>
        readonly char[] POOL = new char[]{ 'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W'
                ,'X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',' ' };
        //Attributs statics
        //Attributs
        /// <summary>
        /// Taille du gene Code
        /// </summary>
        public int MaxLength { get; set; }
        /// <summary>
        /// Code genetic de l'individu qui determinera son pourcentage de fitness
        /// </summary>
        public string GeneCode { get; private set; }
     
        public IndividualString(int length)
        {
            MaxLength = length;
            GeneCode = string.Empty;
            for (int i = 0; i < MaxLength; i++)
            {
                GeneCode += POOL[RndMutateur.Next(0,POOL.Length)];
            }
        }
        IndividualString(string geneCode)
        {
            MaxLength = geneCode.Length;
            GeneCode = geneCode;
        }
        /// <summary>
        /// Creer un individu base sur deux parents avec des chances de mutations
        /// </summary>
        /// <param name="parentA"></param>
        /// <param name="parentB"></param>
        void SwitchCode(int i,int j,IndividualString enfant)
        {
            char[] arrayTemp = enfant.GeneCode.ToArray();
            arrayTemp[i] = enfant.GeneCode[j];
            arrayTemp[j] = enfant.GeneCode[i];
            enfant.GeneCode = string.Empty;
            for (int k = 0; k < MaxLength; k++)
            {
                enfant.GeneCode += arrayTemp[k];
            }
        }

        public override Individual[] ReturnChildrens(Individual partner)
        {
            IndividualString enfantA;
            IndividualString enfantB;
            string geneCodeA = string.Empty;
            string geneCodeB=string.Empty;
            int crossOverPoint = RndMutateur.Next(0, MaxLength / 2);
            char tempA = ' ';
            char tempB= ' ';
            for (int i = 0; i < MaxLength; i++)
            {
                    if (crossOverPoint <= i && i < crossOverPoint + MaxLength / 2)
                    {
                        tempA  = this[i];
                        tempB = partner.GiveInformations()[i];
                    }
                    else
                    {
                        tempA = partner.GiveInformations()[i];
                        tempB = this[i];
                    }

                if (RndMutateur.Next(0, 100) < RANDOM_CHANCE)
                    tempA = POOL[RndMutateur.Next(0, POOL.Length)];
                if (RndMutateur.Next(0, 100) < RANDOM_CHANCE)
                    tempB = POOL[RndMutateur.Next(0, POOL.Length)];

                geneCodeA += tempA;
                geneCodeB += tempB;              
            }
            enfantA = new IndividualString(geneCodeA);
            enfantB = new IndividualString(geneCodeB);

            if (RndMutateur.Next(0, 100) < RANDOM_CHANCE * 5)
                SwitchCode(RndMutateur.Next(0, MaxLength), RndMutateur.Next(0, MaxLength), enfantA);

            if (RndMutateur.Next(0, 100) < RANDOM_CHANCE * 5)
                SwitchCode(RndMutateur.Next(0, MaxLength), RndMutateur.Next(0, MaxLength), enfantB);

            return new IndividualString[] { enfantA, enfantB };
        }

        public override string GiveInformations()
        {
            return GeneCode;
        }

        public char this [int i] { get { return GeneCode[i]; } }

    }
}
