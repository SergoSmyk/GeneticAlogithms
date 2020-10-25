using System;

namespace Labs.api
{
    abstract class Calculation<T, R>
    {
        private GeneticAlgorithm<T, R> algorithm;

        protected int populationSize;

        protected int currentAlgoStep = 0;
        protected abstract bool isEndOfAlgorithm(Individual<T>[] population);

        public Calculation(GeneticAlgorithm<T, R> algorithm, int populationSize)
        {
            this.algorithm = algorithm;
            this.populationSize = populationSize;
        }

        public Individual<T> runAlgorithm()
        {
            currentAlgoStep = 0;

            var population = algorithm.generateStartPopulation(populationSize);

            Individual<T> bestIndividual;

            while (true)
            {
                printCurrentState(population);
                currentAlgoStep++;
                if (isEndOfAlgorithm(population))
                {
                    bestIndividual = algorithm.findBestIndividual(population);
                    break;
                }

                population = algorithm.generateChildren(population);
            }

            return bestIndividual;
        }

        private void printCurrentState(Individual<T>[] population)
        {
            Console.WriteLine("At step {0} : {1}", currentAlgoStep, Tools.arrToString(population));
        }
    }
}
