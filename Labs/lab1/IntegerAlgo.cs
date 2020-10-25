using Labs.api;
using System.Linq;

namespace Labs.lab1
{
    class IntegerAlgo : GeneticAlgorithm<long, double>
    {
        public IntegerAlgo(Function<long, double> function, double mutationChance) : base(function, mutationChance) { }

        protected override Individual<long> createIndividual(long value) => new Individual<long>(value);

        protected override Individual<long>[] createPopulation(int size) => new Individual<long>[size];

        protected override long createRandomValue(long minValue, long maxValue) => Tools.LongRandom(minValue, maxValue, rand);

        public override Individual<long> findBestIndividual(Individual<long>[] population)
        {
            calculateChances(population);

            if (population.Length == 0)
            {
                return null;
            }

            var best = population[0];

            for (int i = 1; i < population.Length; i++)
            {
                best = getBestIndividual(best, population[i]);
            }

            return best;
        }

        public override Individual<long> getBestIndividual(Individual<long> first, Individual<long> second)
        {
            if (first.Chance > second.Chance)
            {
                return first;
            }

            return second;
        }

        public override void calculateChances(params Individual<long>[] population)
        {
            for (int i = 0; i < population.Length; i++)
            {
                calculateChance(ref population[i]);
            }
        }

        public override void calculateChance(ref Individual<long> item)
        {
            item.Chance = func.execute(item.Value);
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
                min: func.MinXValue,
                max: func.MaxXValue);


            return createIndividual(childValue);
        }

        protected override void mutateChild(ref Individual<long> child)
        {
            long mutatedValue = Tools.mutateValue(
                value: child.Value,
                rand: rand,
                min: func.MinXValue,
                max: func.MaxXValue);

            child.mutateValue(mutatedValue);
        }

        public override Individual<long>[] generateChildren(Individual<long>[] population)
        {
            calculateChances(population);
            var newPopulation = createPopulation(population.Length);
            for (int i = 0; i < population.Length; i++)
            {
                var randParent1 = findRandomIndividualByChance(population);
                var randParent2 = findRandomIndividualByChance(population);
                newPopulation[i] = makeChild(randParent1, randParent2);

                if (rand.NextDouble() <= mutationChance)
                {
                    mutateChild(ref newPopulation[i]);
                    calculateChance(ref newPopulation[i]);
                }
            }

            return newPopulation;
        }
    }
}
