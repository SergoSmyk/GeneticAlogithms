using Labs.api;

namespace Labs.lab3
{
    class BackpackProblemCalculations : Calculation<EntitySet, double>
    {
        private long algoStepsCount;
        public BackpackProblemCalculations(
            double maxVolume,
            double mutationChance,
            int populationSize,
            Entity[] entities,
            int algoStepsCount
            ) : base(
                algorithm: new BackpackProblemAlgo(
                    entities: entities,
                    mutationChance: mutationChance,
                    maxVolume: maxVolume),
                populationSize: populationSize
            )
        {
            this.algoStepsCount = algoStepsCount;
        }

        protected override bool isEndOfAlgorithm(Individual<EntitySet>[] population)
        {
            return currentAlgoStep >= algoStepsCount;
        }
    }
}
