using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGeneticAlgo
{
    abstract class Population
    {
        public List<List<Individual>> Generations { get; protected set; }
       protected Random MateSelectorRnd { get; set; }
        /// <summary>
        /// Nombre d'individus dans une generation
        /// </summary>
        public int GenerationSize { get; protected set; }
        public int NbGeneration
        {
            get
            {
                return Generations.Count;
            }
        }
        public Population(int generationSize)
        {
            MateSelectorRnd = new Random();
            Generations = new List<List<Individual>>();
            GenerationSize = generationSize;
        }
        /// <summary>
        /// Cree la premiere generation de facon aleatoire
        /// </summary>
        protected abstract void CreateFirstGeneration();
        /// <summary>
        /// Cree une nouvelle generation en se basant sur la derniere
        /// </summary>
        protected abstract void CreateNewGeneration();
        /// <summary>
        /// Calcule le score de fitness d'un individu
        /// </summary>
        /// <returns></returns>
        protected abstract float CalculateFitnessScore(Individual indiv);
        /// <summary>
        /// Cree nbGenerations nouvelle generations
        /// </summary>
        /// <param name="nbGeneration"></param>
        public abstract void EvolveFor(int nbGeneration);

        public void ShowGenerations()
        {
            for (int i = 0; i < NbGeneration; i++)
            {
                Console.WriteLine("Generation: [{0}]", i + 1);
                Console.WriteLine("[{0}] :Fitness%: {1}", Generations[i][GenerationSize - 1].GiveInformations(), CalculateFitnessScore(Generations[i][GenerationSize - 1]));
                Console.WriteLine(new String('*', 50));
            }
            Console.WriteLine("Nombre de generations: {0}", NbGeneration);
            Console.WriteLine("Meilleur individu de la premiere generation: {0}", Generations[0][GenerationSize - 1].GiveInformations());
        }
    }
}
