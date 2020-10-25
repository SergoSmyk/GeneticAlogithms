using Labs.api;
using System;
using System.Linq;

namespace Labs.lab2
{
    class FloatAlgo : GeneticAlgorithm<double, double>
    {
        private Func<double, double> func;
        private double epsilon;
        private double minValue;
        private double maxValue;
        public FloatAlgo(Func<double, double> func, double minValue, double maxValue, double mutationChance, double epsilon) : base(mutationChance)
        {
            this.func = func;
            this.minValue = minValue;
            this.maxValue = maxValue;
            this.epsilon = epsilon;
        }

        protected override Individual<double> createIndividual(double value) => new Individual<double>(value);

        protected override Individual<double>[] createPopulation(int size) => new Individual<double>[size];

        protected override double createRandomValue()
        {
            var randDouble = Tools.DoubleRandom(minValue, maxValue, rand);

            return randDouble - randDouble % epsilon;
        }       

        public override Individual<double> getBestIndividual(Individual<double> first, Individual<double> second)
        {
            if (first.Chance > second.Chance)
            {
                return first;
            }

            return second;
        }
  
        public override void calculateChance(ref Individual<double> item)
        {
            item.Chance = func.Invoke(item.Value);
        }

        public override double averageChance(Individual<double>[] population)
        {
            return population.Select(item => item.Chance).ToArray().Sum() / population.Length;
        }

        public override Individual<double> makeChild(Individual<double> parent1, Individual<double> parent2)
        {
            long parentBase1 = (long)(parent1.Value / epsilon);
            long parentBase2 = (long)(parent2.Value / epsilon);

            long childBase = Tools.mergeTwoValues(
                a: parentBase1,
                b: parentBase2,
                rand: rand,
                min: (long)(minValue / epsilon),
                max: (long)(maxValue / epsilon));

            return createIndividual(childBase * epsilon);
        }

        protected override void mutateChild(ref Individual<double> child)
        {
            long childBase = (long)(child.Value / epsilon);

            long mutatedValue = Tools.mutateValue(
                value: childBase,
                rand: rand,
                min: (long)(minValue / epsilon),
                max: (long)(maxValue / epsilon));

            child.mutateValue(mutatedValue * epsilon);
        }
    }
}
