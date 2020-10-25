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
                algorithm: new FloatAlgo(function, minValue, maxValue, mutationChance, epsilon),
                populationSize: populationSize
            )
        {
            this.algoStepsCount = algoStepsCount;
        }

        protected override bool isEndOfAlgorithm(Individual<double>[] population)
        {
            return currentAlgoStep >= algoStepsCount;
        }
    }
}
