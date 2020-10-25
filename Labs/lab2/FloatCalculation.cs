using Labs.api;
using System;

namespace Labs.lab2
{
    class FloatCalculation : Calculation<double, double>
    {
        private long algoStepsCount;
        public FloatCalculation(
            long minValue,
            long maxValue,
            Func<double, double> function,
            double mutationChance,
            int populationSize,
            int algoStepsCount,
            double epsilon
            ) : base(
                create(
                    minValue,
                    maxValue,
                    function,
                    mutationChance,
                    epsilon
                ),
                populationSize
            )
        {
            this.algoStepsCount = algoStepsCount;
        }

        private static GeneticAlgorithm<double, double> create(
            long minValue,
            long maxValue,
            Func<double, double> function,
            double mutationChance,
            double epsilon)
        {
            Function<double, double> func = new Function<double, double>(
                minXValue: minValue,
                maxXValue: maxValue,
                function: function
            );

            return new FloatAlgo(func, mutationChance, epsilon);
        }

        protected override bool isEndOfAlgorithm(Individual<double>[] population)
        {
            return currentAlgoStep >= algoStepsCount;
        }
    }
}
