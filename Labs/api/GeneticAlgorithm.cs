using System;

namespace Labs.api
{
    abstract class GeneticAlgorithm<T, R>
    {
        protected static Random rand = new Random();

        protected double mutationChance;

        private double mutationTornamentSize;

        public GeneticAlgorithm(double mutationChance, double mutationTornamentSize = 0.3)
        {
            this.mutationChance = mutationChance;
            this.mutationTornamentSize = mutationTornamentSize;
        }

        protected abstract Individual<T> createIndividual(T value);

        protected abstract void mutateChild(ref Individual<T> child);

        protected abstract Individual<T>[] createPopulation(int size);

        protected abstract T createRandomValue();

        public abstract double averageChance(Individual<T>[] population);

        public virtual void calculateChances(params Individual<T>[] population)
        {
            for(int i = 0; i < population.Length; i++)
            {
                calculateChance(ref population[i]);
            }
        }
        
        public abstract void calculateChance(ref Individual<T> item);

        public abstract Individual<T> makeChild(Individual<T> parent1, Individual<T> parent2);

        public virtual Individual<T> findBestIndividual(Individual<T>[] population)
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

        public abstract Individual<T> getBestIndividual(Individual<T> first, Individual<T> second);

        public Individual<T>[] generateStartPopulation(int size)
        {
            var population = createPopulation(size);

            for (int index = 0; index < size; index++)
            {
                var individualValue = createRandomValue();
                population[index] = createIndividual(individualValue);
            }

            return population;
        }

        public virtual Individual<T>[] generateChildren(Individual<T>[] population)
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

        protected Individual<T> findRandomIndividualByChance(Individual<T>[] population)
        {
            int tournamentSize = Math.Max((int)(population.Length * mutationTornamentSize), 2);
            var tournamentItems = createPopulation(tournamentSize);

            for (int i = 0; i < tournamentSize; i++)
            {
                var randPosition = rand.Next(0, population.Length - 1);
                tournamentItems[i] = population[randPosition];
            }

            return findBestIndividual(tournamentItems);
        }
    }
}