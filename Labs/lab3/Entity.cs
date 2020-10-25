namespace Labs.lab3
{
    struct Entity
    {
        public double Size { get; }
        public double Price { get; }

        public Entity(double size, double price)
        {
            Size = size;
            Price = price;
        }

        public override string ToString()
        {
            return $"[S:{Size},P:{Price}]";
        }
    }
}
