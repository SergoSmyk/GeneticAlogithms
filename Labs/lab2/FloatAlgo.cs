using Labs.api;
using System.Linq;

namespace Labs.lab2
{
    class FloatAlgo : GeneticAlgorithm<double, double>
    {
        private double epsilon;
        public FloatAlgo(Function<double, double> function, double mutationChance, double epsilon) : base(function, mutationChance)
        {
            this.epsilon = epsilon;
        }

        protected override Individual<double> createIndividual(double value) => new Individual<double>(value);

        protected override Individual<double>[] createPopulation(int size) => new Individual<double>[size];

        protected override double createRandomValue(double minValue, double maxValue)
        {
            var randDouble = Tools.DoubleRandom(minValue, maxValue, rand);

            return randDouble - randDouble % epsilon;
        }

        public override Individual<double> findBestIndividual(Individual<double>[] population)
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

        public override Individual<double> getBestIndividual(Individual<double> first, Individual<double> second)
        {
            if (first.Chance > second.Chance)
            {
                return first;
            }

            return second;
        }

        public override void calculateChances(params Individual<double>[] population)
        {
            for (int i = 0; i < population.Length; i++)
            {
                calculateChance(ref population[i]);
            }
        }

        public override void calculateChance(ref Individual<double> item)
        {
            item.Chance = func.execute(item.Value);
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
                min: (long)(func.MinXValue / epsilon),
                max: (long)(func.MaxXValue / epsilon));

            return createIndividual(childBase * epsilon);
        }

        protected override void mutateChild(ref Individual<double> child)
        {
            long childBase = (long)(child.Value / epsilon);

            long mutatedValue = Tools.mutateValue(
                value: childBase,
                rand: rand,
                min: (long)(func.MinXValue / epsilon),
                max: (long)(func.MaxXValue / epsilon));

            child.mutateValue(mutatedValue * epsilon);
        }

        public override Individual<double>[] generateChildren(Individual<double>[] population)
        {
            calculateChances(population);
            var newPopulation = createPopulation(population.Length);
            for (int i = 0; i < population.Length; i++)
            {
                var randParent1 = findRandomIndividualByChance(population);
                var randParent2 = findRandomIndividualByChance(population);
                newPopulation[i] = makeChild(randParent1, randParent2);

                if (rand.NextDouble() <= mutationChance || randParent1.Value == randParent2.Value)
                {
                    mutateChild(ref newPopulation[i]);
                    calculateChance(ref newPopulation[i]);
                }
            }

            return newPopulation;
        }
    }
}
