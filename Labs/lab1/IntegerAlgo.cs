using Labs.api;
using System;
using System.Linq;

namespace Labs.lab1
{
    class IntegerAlgo : GeneticAlgorithm<long, double>
    {
        private Func<long, double> func;

        private long minValue;

        private long maxValue;
        public IntegerAlgo(Func<long, double> function, long minValue, long maxValue, double mutationChance) : base(mutationChance)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
            func = function;
        }

        protected override Individual<long> createIndividual(long value) => new Individual<long>(value);

        protected override Individual<long>[] createPopulation(int size) => new Individual<long>[size];

        protected override long createRandomValue() => Tools.LongRandom(minValue, maxValue, rand);       

        public override Individual<long> getBestIndividual(Individual<long> first, Individual<long> second)
        {
            if (first.Chance > second.Chance)
            {
                return first;
            }

            return second;
        }

        public override void calculateChance(ref Individual<long> item)
        {
            item.Chance = func.Invoke(item.Value);
        }

        public override double averageChance(Individual<long>[] population)
        {
            return population.Select(item => item.Chance).ToArray().Sum() / population.Length;
        }

        public override Individual<long> makeChild(Individual<long> parent1, Individual<long> parent2)
        {
            long childValue = Tools.mergeTwoValues(
                a: parent1.Value,
                b: parent2.Value,
                rand: rand,
                min: minValue,
                max: maxValue);


            return createIndividual(childValue);
        }

        protected override void mutateChild(ref Individual<long> child)
        {
            long mutatedValue = Tools.mutateValue(
                value: child.Value,
                rand: rand,
                min: minValue,
                max: maxValue);

            child.mutateValue(mutatedValue);
        }        
    }
}
