using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGeneticAlgo
{
    class PopulationString:Population
    {
       
        /// <summary>
        /// Critere de selection des individus
        /// </summary>
        public string FitnessCriteria { get; private set; }

        public PopulationString(int generationSize, string fitness):base(generationSize)
        {
            Generations = new List<List<Individual>>();
            GenerationSize = generationSize;
            FitnessCriteria = fitness;
            CreateFirstGeneration();
        }
        /// <summary>
        /// Cree la premiere generation d'individu de facon aleatoire
        /// </summary>
        /// <param name="fitness"></param>
       protected override void CreateFirstGeneration()
        {
            List<Individual> firstGeneration = new List<Individual>();
            for (int i = 0; i < GenerationSize; i++)
            {
                firstGeneration.Add(new IndividualString(FitnessCriteria.Length));
            }
            Generations.Add(firstGeneration);
            Generations[NbGeneration - 1] = Generations[NbGeneration - 1].OrderBy(x => CalculateFitnessScore(x)).ToList();
        }
        /// <summary>
        /// Cree une nouvelle generation en se basant sur les 50% meilleurs individus de la precedente
        /// </summary>
       protected override void CreateNewGeneration()
        {
            List<Individual> tempNewGeneration = new List<Individual>();
            int partnerIndex;
            //La moitie de la population ayant la meilleur fitness sont choisis pour se reproduire.
            for (int i = GenerationSize/2 ;  i < GenerationSize; i++)
            {
                    //A checker si meilleur ignorer les fiable ou les garder pour des gènes recessif possiblement utiles
                    partnerIndex = MateSelectorRnd.Next(0, GenerationSize);
                //On fait deux enfants pour que la totalite du gene code des deux parents se retrouvent dans 
                //la population afin de s'assurer que les bons genes sont conserver
                Individual[] tempChildrenArray = Generations[NbGeneration - 1][i].ReturnChildrens(Generations[NbGeneration - 1][partnerIndex]);
                tempNewGeneration.Add(tempChildrenArray[0]);
                tempNewGeneration.Add(tempChildrenArray[1]);
            }
            Generations.Add(tempNewGeneration);
            Generations[NbGeneration - 1] = Generations[NbGeneration - 1].OrderBy(x => CalculateFitnessScore(x)).ToList();
        }
       public bool EvolveUntil()
        {
            while ( Math.Round(CalculateFitnessScore(Generations[NbGeneration - 1][GenerationSize-1])) !=100)
            {
                Console.WriteLine("Generation: {0}", NbGeneration);
                CreateNewGeneration();
                
            }
            return true;
        }
        protected override float CalculateFitnessScore(Individual indiv)
        {
            float sumGeneHit = 0;
            for (int i = 0; i < FitnessCriteria.Length; i++)
            {
                char test = (indiv as IndividualString).GeneCode[i];
                if (test == FitnessCriteria[i])
                    sumGeneHit++;
            }
            return 100 * (sumGeneHit / FitnessCriteria.Length);
        }

        public override void EvolveFor(int nbGeneration)
        {
            for (int i = 0; i < nbGeneration; i++)
            {
                Console.WriteLine("Generation: {0}", NbGeneration);
                CreateNewGeneration();
            }
        }
    }
}
