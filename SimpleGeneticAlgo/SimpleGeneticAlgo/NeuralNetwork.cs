using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEURAL
{
    class NeuralNetwork
    {
        const double BASE_LEARNING_RATE = 0.01;

        List<Layer> Layers { get; set; }
        public double LearningRate { get; set; }

        public int NbLayers
        {
            get
            {
                return Layers.Capacity;
            }
        }
        public int NbPossibleOutputs
        {
            get
            {
                return Layers[Layers.Count - 1].NbNeurons;
            }
        }
        public int NbNecessaryInputs
        {
            get
            {
                return Layers[0].NbNeurons;
            }
        }
        public Layer OutputLayer
        {
            get
            {
                return Layers[NbLayers - 1];
            }
        }
        public Layer InputLayer
        {
            get
            {
                return Layers[0];
            }
        }
        public NeuralNetwork(int[] structure,double learningRate=BASE_LEARNING_RATE)
        {
            Layers = new List<Layer>(structure.Length);
            //On instancie le premier etage. Il ne contient pas de connexions car il ny a pas detage precedent
            Layers.Add(new Layer(structure[0]));
            //On instancie les etages suivants on mettant le nombre de connexions pour les neurones au nombre de neurones de letage precedent
            for (int i = 1; i < NbLayers; i++)
            {
                Layers.Add(new Layer(structure[i], structure[i - 1]));
            }
        }
        public Matrix FeedForward(double[,] inputs)
        {
            Layers[0].SetValue(new Matrix(inputs));
            for (int i = 1; i < NbLayers; i++)
            {
                Layers[i].SetState(Layers[i-1]);
            }
            return Layers[NbLayers - 1].States;
        }
        void BackPropagate(double[,] inputs, double[,] desiredOutputs)
        {
            Matrix matriceOutputs = new Matrix(desiredOutputs);
            FeedForward(inputs);
            Matrix diff = OutputLayer.States - matriceOutputs;
            //On fait le faux produit entre la difference du ouput donne et celui desirer et le z du dernier etage dans le sigmoid prime
            Matrix delta=Matrix.Schur(OutputLayer.States - matriceOutputs, Matrix.Function(OutputLayer.Values,OutputLayer.Equation.DerivativeOperation));
            for (int i = 0; i <OutputLayer.NbNeurons; i++)
            {
                OutputLayer.Neurons[i].Delta = delta[i, 0];
            }
            for (int i = 1; i <NbLayers; i++)
            {

                for (int j = 0; j < Layers[i].NbNeurons; j++)
                {

                }

            }
        }

    }
}
