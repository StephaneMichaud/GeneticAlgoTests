using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NEURAL
{
    class Layer
    {
        //Attributs
        public List<Node> Neurons { get; private set;}
        public ActivationEquation Equation { get; set; }
        public int NbNeurons
        {
            get
            {
                return Neurons.Count;
            }
        }
        /// <summary>
        /// Valeur des neurones avant quils osivent passer dasn la fonction dactivation
        /// </summary>
        public Matrix Values
        {
            get
            {
                Matrix a = new Matrix(new double[NbNeurons, 1]);
                for (int i = 0; i < NbNeurons; i++)
                {
                    a[i, 0] = Neurons[i].Value;
                }
                return a;
            }
        }
        /// <summary>
        /// Valeur des neurones apres quils osivent passer dasn la fonction dactivation
        /// </summary>
        public Matrix States
        {
            get
            {
                Matrix a = new Matrix(new double[NbNeurons, 1]);
                for (int i = 0; i < NbNeurons; i++)
                {
                    a[i, 0] = Neurons[i].State;
                }
                return a;
            }
        }
        //Constructeurs
        public Layer(int nbNeurons,int nConnections=0)
        {
            Equation = new SigmoidActivation();
            Neurons = new List<Node>(nbNeurons);
            InitialiserNeurons(nConnections);
        }
        public Layer(int nbNeurons, int nConnections,ActivationEquation equation)
        {
            Equation = equation;
            Neurons = new List<Node>(nbNeurons);
            InitialiserNeurons(nConnections);
        }

        /// <summary>
        /// Initialise les nodes dans la liste Neurones
        /// </summary>
        /// <param name="nConnections">Nombre de connexion que les nodes ont avec letage precedent. De base, ils nont pas de connexion</param>
        void InitialiserNeurons(int nConnections = 0)
        {
            for (int i = 0; i < Neurons.Capacity; i++)
            {
                Neurons.Add(new Node(nConnections, 0)); // Le biais est mis a 0 
            }
        }
        public void SetValue(Matrix inputs)
        {
            if (inputs.M != NbNeurons)
                throw new Exception("La taille du inputs ne correspondait pas a la taille du layer");

            for (int i = 0; i < NbNeurons; i++)
            {
                Neurons[i].Value = inputs[i, 0];
            }
        }
        public void SetState(Layer precedent)
        {
            //      if (inputs.M != Neurones.Length)
            //        throw new Exception("La taille du inputs ne correspondait pas a la taille du layer");
            Matrix tempInputs = precedent.States;
            for (int i = 0; i < NbNeurons; i++)
                Neurons[i].Activate(tempInputs);
        }

    }
}
