using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGeneticAlgo
{
    class IndividualNeural : Individual
    {
        const int MUTATION_CHANCE = 2;
        public NeuralNetwork Neural { get; set; }
        public IndividualNeural(NeuralNetwork n)
        {
            Neural = n;
        }


        public override string GiveInformations()
        {
            return string.Empty;
        }

        public override Individual[] ReturnChildrens(Individual partner)
        {
            NeuralNetwork neuralA = new NeuralNetwork(this.Neural);
            NeuralNetwork neuralB = new NeuralNetwork((partner as IndividualNeural).Neural);
            int crossOverPoint = 0;
            for (int layer = 1; layer < neuralA.NbLayers; layer++)
            {
                for (int node = 0; node < neuralA.Layers[layer].NbNeurons; node++)
                {
                    crossOverPoint = RndMutateur.Next(0, neuralA.Layers[layer].Neurons[node].Weights.M / 2);
                    for (int weigth = 0; weigth < neuralA.Layers[layer].Neurons[node].Weights.M; weigth++)
                    {
                        //CrossOver
                        if (weigth >= crossOverPoint && weigth < crossOverPoint + neuralA.Layers[layer].Neurons[node].Weights.M / 2)
                        {
                            neuralB.Layers[layer].Neurons[node].Weights[weigth, 0] = Neural.Layers[layer].Neurons[node].Weights[weigth, 0];
                            neuralA.Layers[layer].Neurons[node].Weights[weigth, 0] = (partner as IndividualNeural).Neural.Layers[layer].Neurons[node].Weights[weigth, 0];
                        }

                        //Modifiaction mutation
                        if(RndMutateur.Next(0,100)< MUTATION_CHANCE)
                            neuralA.Layers[layer].Neurons[node].Weights[weigth, 0] = neuralA.Layers[layer].Neurons[node].Weights[weigth, 0]
                                + RndMutateur.Next(-1,0)==0? (RndMutateur.NextDouble()/100): (-RndMutateur.NextDouble() / 100);

                        if (RndMutateur.Next(0, 100) < MUTATION_CHANCE)
                            neuralB.Layers[layer].Neurons[node].Weights[weigth, 0] = neuralB.Layers[layer].Neurons[node].Weights[weigth, 0]
                                + RndMutateur.Next(-1, 0) == 0 ? (RndMutateur.NextDouble() / 100) : (-RndMutateur.NextDouble() / 100);
                                
                    }
                    //Swap mutation
                    if (RndMutateur.Next(0, 100) < MUTATION_CHANCE)
                        MutateSwapWeigths(ref neuralA, layer, node);
                    if (RndMutateur.Next(0, 100) < MUTATION_CHANCE)
                        MutateSwapWeigths(ref neuralB,layer,node);
                        
                }
            }
            return new Individual[] { new IndividualNeural(neuralA), new IndividualNeural(neuralB) };
        }
        void MutateSwapWeigths(ref NeuralNetwork network,int layer,int node)
        {
           int indexTemp1 = RndMutateur.Next(0, network.Layers[layer].Neurons[node].Weights.M);
           int indexTemp2 = RndMutateur.Next(0, network.Layers[layer].Neurons[node].Weights.M);
           double tempWeigth = network.Layers[layer].Neurons[node].Weights[indexTemp1, 0];
            network.Layers[layer].Neurons[node].Weights[indexTemp1, 0] = network.Layers[layer].Neurons[node].Weights[indexTemp2, 0];
            network.Layers[layer].Neurons[node].Weights[indexTemp2, 0] = tempWeigth;
        }
    }
}
