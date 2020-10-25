using Labs.api;
using System.Linq;

namespace Labs.lab3
{
    class BackpackProblemAlgo : GeneticAlgorithm<EntitySet, double>
    {
        private Entity[] entities;
        private double maxVolume;

        public BackpackProblemAlgo(Entity[] entities, double mutationChance, double maxVolume) : base(mutationChance)
        {
            this.entities = entities;
            this.maxVolume = maxVolume;
        }

        public override double averageChance(Individual<EntitySet>[] population)
        {
            return population.Select((i) => i.Chance).Sum() / population.Length;
        }

        public override void calculateChance(ref Individual<EntitySet> item)
        {
            if (item.Value.Volume <= maxVolume)
            {
                item.Chance = item.Value.Price;
            }
            else
            {
                item.Chance = -item.Value.Price;
            }
        }
                      
        public override Individual<EntitySet> getBestIndividual(Individual<EntitySet> first, Individual<EntitySet> second)
        {
            if (first.Chance > second.Chance)
            {
                return first;
            }
            else if (first.Chance == second.Chance)
            {
                if (first.Value.Volume < second.Value.Volume)
                {
                    return first;
                }               
            }

            return second;
        }

        public override Individual<EntitySet> makeChild(Individual<EntitySet> parent1, Individual<EntitySet> parent2)
        {
            EntitySet childValue = parent1.Value.makeChild(parent2.Value, rand, entities);
            return createIndividual(childValue);
        }

        protected override Individual<EntitySet> createIndividual(EntitySet value) => new Individual<EntitySet>(value);

        protected override Individual<EntitySet>[] createPopulation(int size) => new Individual<EntitySet>[size];

        protected override EntitySet createRandomValue()
        {
            byte[] isEntityIncluded = new byte[entities.Length];
            for(int i = 0; i < entities.Length; i++)
            {
                if (rand.NextDouble() > 0.5)
                {
                    isEntityIncluded[i] = 1;
                }
                else
                {
                    isEntityIncluded[i] = 0;
                }
            }

            return new EntitySet(isEntityIncluded, entities);
        }

        protected override void mutateChild(ref Individual<EntitySet> child)
        {
            child.Value.mutate(rand, entities);      
        }
    }
}
