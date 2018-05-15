using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGeneticAlgo
{
    class Node
    {
        /// <summary>
        /// Equation utiliser pour mettre les vleurs entre 0 et 1
        /// </summary>
        ActivationEquation Equation { get; set; }
        //Attributs
        public double Bias { get; set; }
        /// <summary>
        /// Valeur qui servira a modifier les Weigths et les Biais apres les backpropagate
        /// </summary>
        public double Delta { get; set; }
        /// <summary>
        /// Niveau d'activation du neurone. Valeur situe entre 0 et 1.
        /// </summary>
        public double State { get; set; }
        /// <summary>
        /// Valeur du neurone avant d'avoir passe dans la fonction d'activation.
        /// </summary>
        public double Value { get; set; }
        /// <summary>
        ///Sont les valeurs correspondant a l'importance connections avec les autre neurones des etages precedents
        /// </summary>
        public Matrix Weights { get; private set; }

        // Constructeurs
        public Node(int nConnection=0,int bias=0)
        {
            Equation=new SigmoidActivation();
            Bias = bias;
            Weights = new Matrix(new double[nConnection, 1]);
            for (int i = 0; i < nConnection; i++)
                Weights[i, 0] = CryptoRandom.GetCryptoRandomValue();
        }
        public Node(ActivationEquation equation,int nConnection = 0, int bias = 0)
        {
            Equation = equation;
            Bias = bias;
            Weights = new Matrix(new double[nConnection, 1]);
            for (int i = 0; i < nConnection; i++)
                Weights[i, 0] = CryptoRandom.GetCryptoRandomValue();
        }
        //Operateurs utilise seulement les values
        #region Operators
        public static double operator +(Node A, Node B)
        {
            return A.Value + B.Value;
        }
        public static double operator -(Node A, Node B)
        {
            return A.Value - B.Value;
        }
        public static double operator +(Node A, double B)
        {
            return A.Value + B;
        }
        public static double operator *(Node A, double B)
        {
            return A.Value * B;
        } 
        #endregion
        /// <summary>
        /// On calcule la value et le State selon les valeurs des neurones du layer precedent
        /// </summary>
        /// <param name="inputs">Valeur des states du layer precedent</param>
        public void Activate(Matrix inputs)
        {
            Value=(Matrix.Transpose(inputs) * Weights)[0, 0] + Bias;
            State = Equation.Operation(Value);
        }
    }
    //Utiliser pour ne pas a avoir des randoms a instancier et a eviter le probleme des chiffres pareils quand la clock a pas changer
    public class CryptoRandom
    {
        public static Random rnd = new Random();
        public static double GetCryptoRandomValue()
        {
            //Pour toujours avoir les memes valeurs
            using (RNGCryptoServiceProvider p = new RNGCryptoServiceProvider())
            {
                Random r = new Random(p.GetHashCode());
                return r.NextDouble();
            }
            //return rnd.NextDouble();
        }

    }
    abstract class ActivationEquation
    {
        public abstract double Operation(double x);
        public abstract double DerivativeOperation(double x);
    }
    class SigmoidActivation : ActivationEquation
    {
        public override double Operation(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
        public override double DerivativeOperation(double x)
        {
            return Operation(x)*(1-Operation(x));
        }
    }
}
