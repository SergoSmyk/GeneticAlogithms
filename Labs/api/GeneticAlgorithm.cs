using System;

namespace Labs.api
{
    abstract class GeneticAlgorithm<T, R>
    {
        protected static Random rand = new Random();

        protected double mutationChance;

        private double mutationTornamentSize;

        protected Function<T, R> func;

        public GeneticAlgorithm(Function<T, R> func, double mutationChance, double mutationTornamentSize = 0.3)
        {
            this.func = func;
            this.mutationChance = mutationChance;
            this.mutationTornamentSize = mutationTornamentSize;
        }

        protected abstract Individual<T> createIndividual(T value);

        protected abstract void mutateChild(ref Individual<T> child);

        protected abstract Individual<T>[] createPopulation(int size);

        protected abstract T createRandomValue(T minValue, T maxValue);

        public abstract double averageChance(Individual<T>[] population);

        public abstract void calculateChances(params Individual<T>[] population);

        public abstract void calculateChance(ref Individual<T> item);

        public abstract Individual<T> makeChild(Individual<T> parent1, Individual<T> parent2);

        public abstract Individual<T>[] generateChildren(Individual<T>[] population);

        public abstract Individual<T> findBestIndividual(Individual<T>[] population);

        public abstract Individual<T> getBestIndividual(Individual<T> first, Individual<T> second);

        public Individual<T>[] generateStartPopulation(int size)
        {
            var population = createPopulation(size);

            for (int index = 0; index < size; index++)
            {
                var individualValue = createRandomValue(func.MinXValue, func.MaxXValue);
                population[index] = createIndividual(individualValue);
            }

            return population;
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