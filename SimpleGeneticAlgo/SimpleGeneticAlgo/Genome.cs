using System;

namespace Assets
{
    /// <summary>
    /// Classe utiliser pour du genetic algo. Permet de representer et manipuler dautre classe, dans ce cas ci des neural networks selon des tableau de double
    /// </summary>
    public class Genome
    {
        /// <summary>
        /// 
        /// </summary>
        public const float MUTATION_CHANCE = 0.05f;
        /// <summary>
        /// 
        /// </summary>
        protected Random MutationRnd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        protected Random CrossOverRnd { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public float MutationChance { get; protected set; }

        /// <summary>
        /// Information sur l'individu dune population
        /// </summary>
        public double[] Dna { get;protected set; }
        /// <summary>
        /// 
        /// </summary>
        public int SizeDna { get { return Dna.Length; } }
        public double MinDnaValue { get; set; }
        public double MaxDnaValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dna"></param>
        /// <param name="mutationRnd"></param>
        /// <param name="crossOverRnd"></param>
        public Genome(double[] dna, Random mutationRnd, Random crossOverRnd)
        {
            MutationChance = MUTATION_CHANCE;
            MutationRnd = mutationRnd;
            CrossOverRnd = crossOverRnd;
            Dna = new double[dna.Length];
            for (int i = 0; i < SizeDna; i++)
            {
                Dna[i] = dna[i];
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="c"></param>
        public Genome(Genome c)
        {
            MutationChance = c.MutationChance;
            MutationRnd = c.MutationRnd;
            CrossOverRnd = c.CrossOverRnd;
            MinDnaValue = c.MinDnaValue;
            MaxDnaValue = c.MaxDnaValue;
            Dna = new double[c.SizeDna];
            for (int i = 0; i < SizeDna; i++)
            {
                Dna[i] = c.Dna[i];
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sizeDna"></param>
        /// <param name="mutationRnd"></param>
        /// <param name="crossOverRnd"></param>
        /// <param name="mutationChance"></param>
        public Genome(int sizeDna,Random mutationRnd, Random crossOverRnd,float mutationChance=MUTATION_CHANCE,double minDataValue=-1,double maxDataValue=1)
        {
            MutationChance = mutationChance;
            MutationRnd = mutationRnd;
            MinDnaValue = minDataValue;
            MaxDnaValue = maxDataValue;
            CrossOverRnd = crossOverRnd;
            Dna = new double[sizeDna];
            for (int i = 0; i < SizeDna; i++)
            {
                Dna[i] = MutationRnd.NextDouble()*(MaxDnaValue-minDataValue)+minDataValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double MutateByPercentage(double value)
        {

          //Plus ou moins de 5 a 15 pourcent de la valeur du nombre
            value += value * (0.05+(MutationRnd.NextDouble() / 10)) * MutationRnd.Next(0, 1) == 0 ? -1 : 1;
            return value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double MutateBySign(double value)
        {
            return -value;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double MutateByNumber(double value)
        {
            return value + MutationRnd.NextDouble() * MutationRnd.Next(0,1)==0 ? -1 : 1;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Mutate()
        {
            for (int i = 0; i < Dna.Length; i++)
            {
                if (MutationRnd.NextDouble() < MutationChance)
                    Dna[i] = MutateByPercentage(Dna[i]);
               // if (MutationRnd.NextDouble() < MutationChance)
                 //   Dna[i] = MutateBySign(Dna[i]);

            }
        }

        /// <summary>
        /// Permet de creer deux enfants selon un partenaire choisi en combinant leurs deux adn.
        /// La combinaison des deux adn est fair selon unExemple
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        public virtual Genome[] ProduceOffspring(Genome partner)
        {
            Genome[] r = new Genome[2];
            if (Dna.Length != partner.Dna.Length)
                throw new Exception("Dna non compatible");
            int crossPoint = CrossOverRnd.Next(0, SizeDna);

            Genome Children1 = new Genome(this);
            Genome Children2 = new Genome(this);
            for (int i = 0; i < SizeDna; i++)
            {
                if (i < crossPoint)
                {
                    Children1.Dna[i] = partner.Dna[i];
                }
                else
                {
                    Children2.Dna[i] = partner.Dna[i];
                }
            }
            Children1.Mutate();
            Children2.Mutate();
            r[0] = Children1;
            r[0] = Children2;
            return r;

        }
        /// <summary>
        /// 
        /// </summary>
        public void Show()
        {
            for (int i = 0; i < Dna.Length; i++)
            {
                Console.Write("[" + Math.Round(Dna[i], 3) + "] ");
            }
        }
    }
}
