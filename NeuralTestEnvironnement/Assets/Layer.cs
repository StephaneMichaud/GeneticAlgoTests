using System;
namespace Assets
{
    public class Layer
    {
        //Attributs
        /// <summary>
        /// 
        /// </summary>
        public  Neuron[] Neurons { get; private set;}
        /// <summary>
        /// 
        /// </summary>
        public IActivationEquation Equation { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double[] Output { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int NbNeurons
        {
            get
            {
                return Neurons.Length;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nbNeurons"></param>
        /// <param name="nConnections"></param>
        public Layer(int nbNeurons,int nConnections=0)
        {
            Equation = new SigmoidActivation();
            Neurons = new Neuron[nbNeurons];
            InitialiserNeurons(nConnections);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nbNeurons"></param>
        /// <param name="nConnections"></param>
        /// <param name="equation"></param>
        public Layer(int nbNeurons, int nConnections,IActivationEquation equation)
        {
            Equation = equation;
            Neurons = new Neuron[nbNeurons];
            InitialiserNeurons(nConnections);
        }

        /// <summary>
        /// Initialise les nodes dans la liste Neurones
        /// </summary>
        /// <param name="nConnections">Nombre de connexion que les nodes ont avec letage precedent. De base, ils nont pas de connexion</param>
        void InitialiserNeurons(int nConnections = 0)
        {
            for (int i = 0; i < NbNeurons; i++)
            {
                Neurons[i] = new Neuron(Equation, nConnections);
                Neurons[i].Randomize();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public double[] Compute(double[] inputs)
        {
            // local variable to avoid mutlithread conflicts
            double[] output = new double[NbNeurons];

            // compute each neuron
            for (int i = 0; i < Neurons.Length; i++)
                output[i] = Neurons[i].Compute(inputs);

            // assign output property as well (works correctly for single threaded usage)
            if (Equation is SoftMaxActivation)
            {
                (Equation as SoftMaxActivation).CalculerDiviseur(output); 
                //on divise par la somme calculer
                for (int i = 0; i < Neurons.Length; i++)
                    output[i] = Neurons[i].SetOutput(output[i] / (Equation as SoftMaxActivation).Diviseur);
                //pour reset le diviseur
                Equation = new SoftMaxActivation();
            }
            this.Output = output;

            return output;
        }
        /// <summary>
        /// Utiliser pour le premier layer quand il recoit les inputs
        /// </summary>
        /// <param name="inputs"></param>
        public void SetOutput(double[] inputs)
        {
            if (inputs.Length != NbNeurons)
                throw new Exception("Le nombre de inputs ne correspond pas au nombre de neurons");
            for (int i = 0; i < NbNeurons; i++)
            {
                Neurons[i].SetOutput(inputs[i]);
            }
           
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="precedent"></param>
        /// <returns></returns>
        public double[] Compute(Layer precedent)
        {
            //pareil que lautre mais on peut envoyer un layer 
            double[] output = new double[NbNeurons];
            for (int i = 0; i < Neurons.Length; i++)
                output[i]=Neurons[i].Compute(precedent.Output);
            if(Equation is SoftMaxActivation)
            {
                (Equation as SoftMaxActivation).CalculerDiviseur(output);
                //on divise par la somme calculer
                for (int i = 0; i < Neurons.Length; i++)
                    output[i] = Neurons[i].SetOutput(output[i]/(Equation as SoftMaxActivation).Diviseur);
                //pour reset le diviseur
                Equation = new SoftMaxActivation();
            }
            this.Output = output;
            return output;
        }
        /// <summary>
        /// Set new activation function for all neurons of the layer.
        /// </summary>
        /// 
        /// <param name="function">Activation function to set.</param>
        /// 
        /// <remarks><para>The methods sets new activation function for each neuron by setting
        /// their <see cref="ActivationNeuron.ActivationFunction"/> property.</para></remarks>
        /// 
        public void SetActivationFunction(IActivationEquation function)
        {
            Equation = function;
            for (int i = 0; i < Neurons.Length; i++)
            {
                Neurons[i].SetActivationEquation(ref function);
            }
        }

    }
}
