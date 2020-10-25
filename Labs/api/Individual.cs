using System.Collections.Generic;

namespace Labs.api
{
    class Individual<T>
    {
        private T value;
        public T Value
        {
            get { return value; }
        }

        private double chance;

        public double Chance
        {
            get { return chance; }

            set { this.chance = value; }
        }

        public Individual(T value)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }

        public override bool Equals(object obj)
        {
            return obj is Individual<T> individual &&
                   EqualityComparer<T>.Default.Equals(value, individual.value);
        }

        public void mutateValue(T newValue)
        {
            this.value = newValue;
        }
    }
}
