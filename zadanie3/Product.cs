using System;
namespace zadanie3
{
    public class Product
    {
        private int Asin { get; }
        private int[] Ratings { get; }

        public Product(int asin, int[] ratings)
        {
            this.Asin = asin;
            this.Ratings = ratings;
        }
    }
}
