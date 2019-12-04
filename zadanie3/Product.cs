using System.Collections.Generic;

namespace zadanie3
{
    public class Product
    {
        private string Asin { get; }
        private Dictionary<string, int> Reviews { get; }

        public Product(string asin, Dictionary<string, int> reviews)
        {
            Asin = asin;
            Reviews = reviews;
        }

        public override string ToString()
        {
            string s = "\nProduct ASIN: " + Asin + "\nReviews: " + Reviews.Count + "\n";
            foreach (KeyValuePair<string, int> entry in Reviews)
            {
                s += "customer: " + entry.Key + " | ";
                s += "rating: " + entry.Value + "\n";
            }
            return s;
        }
    }
}
