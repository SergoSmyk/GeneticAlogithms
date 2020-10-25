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
                algorithm: new IntegerAlgo(function, minValue, maxValue, mutationChance),               
                populationSize: populationSize
            )
        {
            this.algoStepsCount = algoStepsCount;
        }

        protected override bool isEndOfAlgorithm(Individual<long>[] population)
        {
            return currentAlgoStep >= algoStepsCount;
        }
    }
}
