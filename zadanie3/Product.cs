using System.Collections.Generic;

namespace zadanie3
{
    public class Product
    {
        public int Id { get; }
        public Dictionary<int, int> Reviews { get; }

        public Product(int id, Dictionary<int, int> reviews)
        {
            Id = id;
            Reviews = reviews;
        }

        public override string ToString()
        {
            string s = "\nProduct Id: " + Id + "\nReviews: " + Reviews.Count + "\n";
            foreach (KeyValuePair<int, int> entry in Reviews)
            {
                s += "customer: " + entry.Key + " | ";
                s += "rating: " + entry.Value + "\n";
            }
            return s;
        }
    }
}
