using System;
using System.Linq;
using System.Security.Cryptography;

namespace Assets
{
    public class Neuron
    {
        /// <summary>
        /// Equation utiliser pour mettre les valeurs entre 0 et 1
        /// </summary>
        public IActivationEquation Equation { get;private set; }
        //Attributs
        public double Bias { get; set; }
        /// <summary>
        /// Valeur qui servira a modifier les Weigths et les Biais apres les backpropagate
        /// </summary>
        public double Delta { get; set; }
        /// <summary>
        /// Niveau d'activation du neurone. Valeur situe entre 0 et 1.
        /// </summary>
        public double Output { get; set; }

        //Pour controller dans quel intervalle se situe les valeurs aleatoires
        double minValueRand = -1;
        public double MinValueRand { get { return minValueRand; } set { minValueRand = value; } }
        double maxValueRand = 1;
        public double MaxValueRand { get { return maxValueRand; } set { maxValueRand = value; } }
        public double RandRange { get {return MaxValueRand - MinValueRand; } }
        /// <summary>
        /// 
        /// </summary>
        public int InputCounts { get { return Weights.Length; } }
        /// <summary>
        ///Sont les valeurs correspondant a l'importance connections avec les autre neurones des etages precedents
        /// </summary>
        public double[] Weights { get; private set; }

        // Constructeurs
        public Neuron(int nConnection=0)
        {
            Equation=new SigmoidActivation();
            Weights = new double[nConnection];
            Randomize();
           
        }
        public Neuron(IActivationEquation equation,int nConnection = 0)
        {
            Equation = equation;
            Weights = new double[nConnection];
            Randomize();

        }
        public Neuron(Neuron n)
        {
            Weights = new double[n.InputCounts];
            for (int i = 0; i < InputCounts; i++)
            {
                Weights[i] = n.Weights[i];
            }
            Bias = n.Bias;
        }
        public void Randomize()
        {
            double d = RandRange;

            // randomize weights
            for (int i = 0; i < InputCounts; i++)
                Weights[i] = CryptoRandom.GetRandom() * d + MinValueRand;
            //randomize biais
            Bias = CryptoRandom.GetRandom() *d + MinValueRand;
            Bias = 10 * Bias;
        }
        
        /// <summary>
        /// On calcule la value et le State selon les valeurs des neurones du layer precedent
        /// </summary>
        /// <param name="inputs">Valeur des parametres connecter au weights</param>
        public double Compute(double[] inputs)
        {
            // check for corrent input vector
            if (inputs.Length != InputCounts)
                throw new ArgumentException("Mauvaise taille du vecteur inputs ");

            // valleur sum initial
            double sum = 0.0;

            // on multiplie les inputs avec les weights correpondants
            for (int i = 0; i < Weights.Length; i++)
            {
                sum += Weights[i] * inputs[i];
            }
            //on additionne le biais
            sum += Bias;

            // on met la somme de la fontion d'activation 
            double output = Equation.Operation(sum);
            // ont me le ouput en memoire
            this.Output = output;

            return output;
        }
        /// <summary>
        /// On calcule la value et le State selon les valeurs des neurones du layer precedent
        /// </summary>
        /// <param name="inputs">Valeur des parametres connecter au weights</param>
        public double SetOutput(double input)
        {
            this.Output = input;
            return input;
        }

        public void SetActivationEquation(ref IActivationEquation a)
        {
            Equation = a;
        }
    }
    //Utiliser pour ne pas a avoir des randoms a instancier et a eviter le probleme des chiffres pareils quand la clock a pas changer
    public class CryptoRandom
    {
        public static Random rnd = new Random();
        public static double GetCryptoRandomValue()
        {
            //Pour toujours avoir les memes valeurs
           // using (RNGCryptoServiceProvider p = new RNGCryptoServiceProvider())
            //{
              //  Random r = new Random(p.GetHashCode());
                return rnd.NextDouble();
            //}
        }
        public static double GetRandom()
        {
            return rnd.NextDouble();
        }

    }
    /// <summary>
    /// Classe de base utiliser pour les fonction d'activation des neurals networks
    /// </summary>
   public interface IActivationEquation
    {
         double Operation(double x);
         double DerivativeOperation(double x);
    }
   public  class SigmoidActivation : IActivationEquation
    {
        public double Alpha { get; set; }
        public SigmoidActivation(double alpha=2)
        {
            Alpha = alpha;
        }

        public double Operation(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
        public double DerivativeOperation(double x)
        {
            return Operation(x)*(1-Operation(x));
        }
        public object Clone()
        {
            return new SigmoidActivation(Alpha);
        }
    }
    public class BipolarSigmoidActivation : IActivationEquation
    {
        public double Alpha { get; set; }
        public BipolarSigmoidActivation(double alpha = 2)
        {
            Alpha = alpha;
        }

        public double DerivativeOperation(double x)
        {
            double y = Operation(x);

            return (Alpha * (1 - y * y) / 2);
        }

        public double Operation(double x)
        {
            return ((2 / (1 + Math.Exp(-Alpha * x))) - 1);
        }
    }
    public class HyperbolicTanActivation : IActivationEquation
    {
        public double DerivativeOperation(double x)
        {
            return 1 - Math.Pow(Math.Tanh(x), 2);
        }

        public double Operation(double x)
        {
            return Math.Tanh(x);
        }
    }
    public class ReLuActivation : IActivationEquation
    {
        public double DerivativeOperation(double x)
        {
            return x<0 ? 0 : 1;
        }

        public double Operation(double x)
        {
            return Math.Max(0, x);
        }
        /// <summary>
        /// Doit etre utiliser pour le output layer
        /// </summary>
        /// <param name="output"></param>
        /// <returns></returns>
        public double[] ReturnSoftMax(double[] output)
        {
            double diviseur = 0;
            for (int i = 0; i < output.Length; i++)
            {
                output[i] = Math.Exp(output[i]);
                diviseur += output[i];
            }
            for (int i = 0; i < output.Length; i++)
            {
                output[i] /= diviseur;
            }
            return output;
        }
    }
    public class SoftMaxActivation : IActivationEquation
    {
        public double Diviseur { get; set; }
        public double DerivativeOperation(double x)
        {
            //a implementer
            return 0;
        }

        public double Operation(double x)
        {
            return Math.Exp(x);
        }
        public void CalculerDiviseur(double[] output)
        {
            Diviseur= output.Sum();
        }
    }
    public class Tuple<T1, T2>
    {
        public T1 First { get; private set; }
        public T2 Second { get; private set; }
        internal Tuple(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }

    public static class Tuple
    {
        public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
        {
            var tuple = new Tuple<T1, T2>(first, second);
            return tuple;
        }
    }

}
