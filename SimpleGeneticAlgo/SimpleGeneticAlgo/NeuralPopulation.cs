using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets
{
    public class NetworkPopulation
    {
        /// <summary>
        /// 
        /// </summary>
        public NeuralNetwork NeuralNetwork { get; private set; }
        /// <summary>
        /// 
        /// </summary>
       public List<NetworkGenome[]> Generations { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int GenerationSize { get; private set;}
        /// <summary>
        /// 
        /// </summary>
        public int NbGeneration { get { return Generations.Count; } }
        /// <summary>
        /// 
        /// </summary>
        Random MateSelectorRnd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        Random MutationRnd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        Random CrossOverRnd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float ElitePercent { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public float LoserPercent { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public float ReproductionPercent { get;protected set; }
        /// <summary>
        /// 
        /// </summary>
        public float MutationChance { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="generationSize"></param>
        /// <param name="net"></param>
        public NetworkPopulation(int generationSize, NeuralNetwork net,float mutationChance, float elitePercent = 0.1f,float loserPercent=0.1f ,float reproductionPercent = 0.8f)
        {
            ElitePercent = elitePercent;
            LoserPercent = loserPercent;
            ReproductionPercent = reproductionPercent;
            NeuralNetwork = net;
            MutationChance = mutationChance;
            MateSelectorRnd = new Random();
            MutationRnd = new Random();
            CrossOverRnd = new Random();
            GenerationSize = generationSize;
            Generations = new List<NetworkGenome[]>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void CreateFirstGeneration()
        {
            NetworkGenome[] generationTemp = new NetworkGenome[GenerationSize];
            for (int i = 0; i < GenerationSize; i++)
            {
                generationTemp[i] = new NetworkGenome(NeuralNetwork.Layout, NeuralNetwork.NbNecessaryInputs, 
                    NeuralNetwork.NetworkSize, MutationRnd, CrossOverRnd, MutationChance,NeuralNetwork.Layers[0].Neurons[0].MinValueRand, NeuralNetwork.Layers[0].Neurons[0].MaxValueRand);
            }
            Generations.Add(generationTemp);
        }
        /// <summary>
        /// 
        /// </summary>
        public void CreateNewGeneration()
        {

            Generations[NbGeneration-1]= Generations[NbGeneration - 1].OrderByDescending(x => x.Fitness).ToArray();
            //Checker si shallow copy
            NetworkGenome[] generationTemp = new NetworkGenome[GenerationSize];
            NetworkGenome[] elites = GetElites(NbGeneration);

            float totalFitness =GetTotalFitness(NbGeneration);

            
            int indexNewGen = 0;
            int indexPartner = -1;

            //garde lelite
            for (int i = 0; i < elites.Length && indexNewGen<GenerationSize; i++)
            {
                generationTemp[indexNewGen++] = elites[i];
            }
            //rajoute random pour diversiter
            for (int i = 0; i < (int)(LoserPercent*GenerationSize)&&indexNewGen<GenerationSize; i++)
            {
                generationTemp[indexNewGen++] = new NetworkGenome(NeuralNetwork.Layout, NeuralNetwork.NbNecessaryInputs, NeuralNetwork.NetworkSize, MutationRnd, CrossOverRnd, MutationChance);
            }

            //fill avec des enfants
            while( indexNewGen < GenerationSize )
            {
                indexPartner = ChooseMateIndex(NbGeneration);
                Genome[] enfants = Generations[NbGeneration - 1][indexPartner].ProduceOffspring(Generations[NbGeneration - 1][ChooseMateIndex(NbGeneration,indexPartner)]);
                generationTemp[indexNewGen++] = (enfants[0] as NetworkGenome);
                generationTemp[indexNewGen++] = (enfants[1] as NetworkGenome);
            }
          
            Generations.Add(generationTemp);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gen"></param>
        /// <param name="indexPartner"></param>
        /// <returns></returns>
        int ChooseMateIndex(int gen,int indexPartner)
        {
            int index = 0;
            float totalFitness = GetTotalFitness();
            float mateNumber = 0;
            do
            {
                index = 0;
                mateNumber = (float)MateSelectorRnd.NextDouble();
               
                while (mateNumber > 0)
                {
                    mateNumber -= (Generations[gen - 1][index].Fitness) / totalFitness;
                    index++;
                }
               
                index--;
            }
            while (index==indexPartner);
            return index;
        }
        int ChooseMateIndex(int gen)
        {
            int index = 0;
            float mateNumber = (float)MateSelectorRnd.NextDouble();
            float totalFitness = GetTotalFitness();
            while (mateNumber>0)
            {
                mateNumber -= Generations[gen - 1][index].Fitness/totalFitness;
                index++;
            } 
            return --index;

        }
        NetworkGenome ChooseMate()
        {
            int index = 0;
            float mateNumber = (float)MateSelectorRnd.NextDouble();
            float totalFitness = GetTotalFitness();
            do
            {
                mateNumber -= Generations[NbGeneration - 1][index].Fitness / totalFitness;
                index++;
            } while (mateNumber > 0);
            return Generations[NbGeneration - 1][--index];

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fitnessFunction"></param>
        public void ApplyFitnessFunction(Func<NetworkGenome,float> fitnessFunction)
        {
            // a changer
            for (int i = 0; i < GenerationSize; i++)
            {
                if(Generations[NbGeneration - 1][i].Fitness==-1)
                    Generations[NbGeneration - 1][i].Fitness = fitnessFunction(Generations[NbGeneration - 1][i]);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gen"></param>
        /// <returns></returns>
        public NetworkGenome[] GetElites(int gen)
        {
            if (gen == 0)
                throw new Exception("Les generations commence a 1");
            if (Generations.Count == 0)
                return null;
            return Generations[gen - 1].OrderByDescending(x => x.Fitness).Take((int)(GenerationSize * ElitePercent)).ToArray();
        }
        public float GetTotalFitness(int gen)
        {
            if (gen == 0)
                throw new Exception("Les generations commence a 1");
            if (Generations.Count == 0)
                return 0;
            return Generations[gen - 1].Sum(x => x.Fitness);
        }
        public float GetTotalFitness()
        {
            if (NbGeneration == 0)
                throw new Exception("Les generations commence a 1");
            if (Generations.Count == 0)
                return 0;
            return Generations[NbGeneration - 1].Sum(x => x.Fitness);
        }

        public float GetHighestFitness(int gen)
        {
            return Generations[gen - 1].Max(x => x.Fitness);
        }
        public float GetHighestFitness()
        {
            return Generations[NbGeneration - 1].Max(x => x.Fitness);
        }
        public NeuralNetwork GetBestNetwork(int gen)
        {
            float maxZ = Generations[gen-1].Max(obj => obj.Fitness);
            return  Generations[gen-1].Where(obj => obj.Fitness == maxZ).ToList()[0].ReturnNetwork(NeuralNetwork);
        }

    }
}
