using System;

namespace Assets
{
    public class NetworkGenome : Genome
    {
        /// <summary>
        /// 
        /// </summary>
        public int[] Layout { get; set; }
        IActivationEquation Equation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int NbInputs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float Fitness { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        /// <param name="layout"></param>
        /// <param name="nbInputs"></param>
        public NetworkGenome(Genome c,int[] layout,int nbInputs) : base(c)
        {
            Layout = layout;
            NbInputs = nbInputs;
            Fitness = -1;
        }
        public NetworkGenome(NetworkGenome genom):
            base(genom.Dna,genom.MutationRnd,genom.CrossOverRnd)
        {
            Equation = genom.Equation;
            Layout = genom.Layout;
            NbInputs = genom.NbInputs;
            Fitness = genom.Fitness;
        }
        public NetworkGenome(int[] layout, int nbInputs,int tailleNetwork,Random mutationRnd,Random CrossOverRnd,float mutationChance = MUTATION_CHANCE,double minDataValue=-1,double maxDataValue=1) :
            base(tailleNetwork,mutationRnd,CrossOverRnd, mutationChance,minDataValue,maxDataValue)
        {
            Layout = layout;
            NbInputs = nbInputs;
            Fitness = -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dna"></param>
        /// <param name="mutationRnd"></param>
        /// <param name="crossOverRnd"></param>
        /// <param name="layout"></param>
        /// <param name="nbInputs"></param>
        public NetworkGenome(double[] dna, Random mutationRnd, Random crossOverRnd,int[] layout,int nbInputs) : 
            base(dna, mutationRnd, crossOverRnd)
        {
            Layout = layout;
            NbInputs = nbInputs;
            Fitness = -1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="n"></param>
        /// <param name="mutationRnd"></param>
        /// <param name="crossOverRnd"></param>
        /// <param name="mutationChance"></param>
        public NetworkGenome(NeuralNetwork n, Random mutationRnd, Random crossOverRnd, float mutationChance=MUTATION_CHANCE)
            :base(n.TotalNumberBiais+n.TotalNumberWeights,mutationRnd,crossOverRnd,mutationChance,n.Layers[0].Neurons[0].MinValueRand, n.Layers[0].Neurons[0].MaxValueRand)
        {
            Layout = n.Layout;
            NbInputs = n.NbNecessaryInputs;
            int indexDna = 0;
            for (int i = 0; i < n.Layers.Length; i++)
            {
                for (int j = 0; j < n.Layers[i].Neurons.Length; j++)
                {
                    Dna[indexDna++] = n.Layers[i].Neurons[j].Bias;
                    for (int k = 0; k < n.Layers[i].Neurons[j].Weights.Length; k++)
                    {
                        Dna[indexDna++] = n.Layers[i].Neurons[j].Weights[k];
                    }
                }
            }
            Fitness = -1;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public NeuralNetwork ReturnNetwork(NeuralNetwork n)
        {
            NeuralNetwork tempNetwork = new NeuralNetwork(NbInputs, Layout);
            int indexDna = 0;
            for (int i = 0; i < tempNetwork.Layers.Length; i++)
            {
                for (int j = 0; j < tempNetwork.Layers[i].Neurons.Length; j++)
                {
                    tempNetwork.Layers[i].Neurons[j].Bias = Dna[indexDna++];
                    for (int k = 0; k < tempNetwork.Layers[i].Neurons[j].Weights.Length; k++)
                    {
                        tempNetwork.Layers[i].Neurons[j].Weights[k] = Dna[indexDna++];
                    }
                }
            }
            tempNetwork.SetActivationFunction(n.ActivationEquation);
            return tempNetwork;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="network"></param>
        public void SetNetwork(ref NeuralNetwork network)
        {
            if (NbInputs != network.NbNecessaryInputs)
                throw new Exception("Nombre de inputs non compatible");
            if (Layout != network.Layout)
                throw new Exception("Structure du network non compatible");
            int indexDna = 0;
            for (int i = 0; i < network.Layers.Length; i++)
            {
                for (int j = 0; j < network.Layers[i].Neurons.Length; j++)
                {
                    network.Layers[i].Neurons[j].Bias = Dna[indexDna++];
                    for (int k = 0; k < network.Layers[i].Neurons[j].Weights.Length; k++)
                    {
                        network.Layers[i].Neurons[j].Weights[k] = Dna[indexDna++];
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public override Genome[] ProduceOffspring(Genome partner)
        {
            Genome[] r = new Genome[2];
            if (Dna.Length != partner.Dna.Length)
            {
                //if (partner is NetworkGenome)
                  //  return ProduceOffspringNeat(partner as NetworkGenome);
                //else
                  //  throw new Exception("Les deux chromosones ne sont pas compatibles");
            }

            NetworkGenome Children1 = new NetworkGenome(this);
            NetworkGenome Children2 = new NetworkGenome(this);

            int indexDna = 0;
            int crossPoint = 0; ;
            for (int layer = 0; layer < this.Layout.Length; layer++)
            {
                for (int node = 0; node < this.Layout[layer]; node++)
                {
                    //pour skip les biais A CHECKER 
                    indexDna++;
                    crossPoint = CrossOverRnd.Next(0, ((layer == 0) ? NbInputs : Layout[layer - 1]));
                    for (int weigths = 0; weigths < ((layer==0) ? NbInputs:Layout[layer-1]); weigths++)
                    {
                        if (weigths < crossPoint)
                            Children1.Dna[indexDna] = partner.Dna[indexDna];
                        else
                            Children2.Dna[indexDna] = partner.Dna[indexDna];
                        indexDna++;
                    }
                }
            }
            Children1.Mutate();
            Children2.Mutate();
            Children1.Fitness = -1;
            Children2.Fitness = -1;
            r[0] = Children1;
            r[1] = Children2;
            return r;
        }
        public Tuple<Genome, Genome> ProduceOffspringNeat(NetworkGenome partner)
        {
            throw new NotImplementedException();
        }
    }
}
