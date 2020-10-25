using Labs.api;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Labs.lab3
{
    class EntitySet
    {
        private bool isValuesInitialized = false;

        private byte[] isEntityUsed; // if 1 used 0 not used

        private double price;
        
        public double Price
        {
            get => price;
        }

        private double volume;

        public double Volume
        {
            get => volume;
        }

        public EntitySet(byte[] isEntityUsed, Entity[] entities)
        {
            this.isEntityUsed = isEntityUsed;
            calculatePriceAndVolume(entities);
        }

        public override string ToString()
        {
            return Tools.arrToString(isEntityUsed);
        }

        public void mutate(Random rand, Entity[] entities)
        {
            int mutateIndex = rand.Next(0, isEntityUsed.Length);
            isEntityUsed[mutateIndex] = (byte) Math.Abs(isEntityUsed[mutateIndex] - 1);
            calculatePriceAndVolume(entities);
        }

        public EntitySet makeChild(EntitySet other, Random rand, Entity[] entities)
        {
            int crossoverIndex = rand.Next(0, entities.Length - 1);
            byte[] childArray = new byte[isEntityUsed.Length];

            for (int i = 0; i < isEntityUsed.Length; i++)
            {
                if (i <= crossoverIndex)
                {
                    childArray[i] = isEntityUsed[i];
                }
                else
                {
                    childArray[i] = other.isEntityUsed[i];
                }
            }

            return new EntitySet(childArray, entities);
        }

        public String getFormatedString(Entity[] entities)
        {
            List<Entity> usedEntities = new List<Entity>();
            for (int i = 0; i < entities.Length; i++)
            {
                if (isEntityUsed[i] == 1)
                {
                    usedEntities.Add(entities[i]);
                }
            }

            String itemsLine = Tools.arrToString(usedEntities.ToArray<Entity>());

            return $"Total Price: {Price}, List: {itemsLine}";
        } 

        private void calculatePriceAndVolume(Entity[] entities)
        {           
            price = 0d;
            volume = 0d;

            for (int i = 0; i < isEntityUsed.Length; i++)
            {
                if (isEntityUsed[i] == 1)
                {
                    price += entities[i].Price;
                    volume += entities[i].Size;
                }
            }
        }
    }
}
