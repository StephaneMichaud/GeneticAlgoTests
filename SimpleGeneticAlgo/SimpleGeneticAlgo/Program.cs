using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGeneticAlgo
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] structN = new int[]{4,2,2};
            IndividualNeural parentA =new IndividualNeural( new NeuralNetwork(structN));
            IndividualNeural parentB = new IndividualNeural(new NeuralNetwork(structN));
            Individual[] enfants = parentA.ReturnChildrens(parentB);
            int stop = 1;
        }
    }
}
