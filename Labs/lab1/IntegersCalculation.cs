using Labs.api;
using System;

namespace Labs.lab1
{
    class IntegersCalculation : Calculation<long, double>
    {
        private long algoStepsCount;
        public IntegersCalculation(
            long minValue,
            long maxValue,
            Func<long, double> function,
            double mutationChance,
            int populationSize,
            int algoStepsCount
            ) : base(
                create(
                    minValue,
                    maxValue,
                    function,
                    mutationChance
                ),
                populationSize
            )
        {
            this.algoStepsCount = algoStepsCount;
        }

        private static GeneticAlgorithm<long, double> create(
            long minValue,
            long maxValue,
            Func<long, double> function,
            double mutationChance)
        {
            Function<long, double> func = new Function<long, double>(
                minXValue: minValue,
                maxXValue: maxValue,
                function: function
            );

            return new IntegerAlgo(func, mutationChance);
        }

        protected override bool isEndOfAlgorithm(Individual<long>[] population)
        {
            return currentAlgoStep >= algoStepsCount;
        }
    }
}
