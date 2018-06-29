namespace Assets
{
    public class NeuralNetwork
    {
        const double BASE_LEARNING_RATE = 0.01;
        /// <summary>
        /// 
        /// </summary>
        public Layer[] Layers { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public int[] Layout { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public double LearningRate { get; set; }
        public IActivationEquation ActivationEquation {get;set;}

        /// <summary>
        /// Nombre de hidden layers plus le output layer
        /// </summary>
        public int NbLayers
        {
            get
            {
                return Layers.Length;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int NbPossibleOutputs
        {
            get
            {
                return Layers[Layers.Length - 1].NbNeurons;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int NbNecessaryInputs { get;private set; }
        /// <summary>
        /// 
        /// </summary>
        public double[] Output { get; protected set; }
        /// <summary>
        /// 
        /// </summary>
        public int TotalNumberWeights
        {
            get
            {
                int total = 0;
                for (int i = 0; i < Layers.Length; i++)
                {
                    for (int j = 0; j < Layers[i].Neurons.Length; j++)
                    {
                        total += Layers[i].Neurons[j].Weights.Length;
                    }
                }
                return total;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public int TotalNumberBiais
        {
            get
            {
                int total = 0;
                for (int i = 0; i < Layers.Length; i++)
                {
                    total += Layers[i].Neurons.Length;
                }
                return total;
            }
        }
        public int NetworkSize { get { return TotalNumberBiais + TotalNumberWeights; } }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nbInputs"></param>
        /// <param name="structure"></param>
        /// <param name="learningRate"></param>
        public NeuralNetwork(int nbInputs,int[] structure,double learningRate=BASE_LEARNING_RATE)
        {
            ActivationEquation = new SigmoidActivation();
            NbNecessaryInputs = nbInputs;
            Layout = structure;
            Layers = new Layer[structure.Length];
            //On instancie le premier etage des hidden layer. 
            Layers[0] = new Layer(structure[0],NbNecessaryInputs);
            //On instancie les etages suivants on mettant le nombre de connexions pour les neurones au nombre de neurones de letage precedent
            for (int i = 1; i < NbLayers; i++)
            {
                Layers[i]=new Layer(structure[i], structure[i - 1]);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public double[] Compute(double[] inputs)
        {
            double[] output = inputs;
            for (int i = 0; i < NbLayers; i++)
            {
                output=Layers[i].Compute(output);
            }
            this.Output = output;
            return Layers[NbLayers - 1].Output;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="desiredOutputs"></param>
        void BackPropagate(double[,] inputs, double[,] desiredOutputs)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="function"></param>
        public void SetActivationFunction(IActivationEquation function)
        {
            ActivationEquation = function;
            for (int i = 0; i < NbLayers; i++)
            {
                if (function is ReLuActivation)
                {
                    if (i == NbLayers - 1)
                        Layers[i].SetActivationFunction(new SoftMaxActivation());
                }
                else
                    Layers[i].SetActivationFunction(function);

            }
        }

    }
}
