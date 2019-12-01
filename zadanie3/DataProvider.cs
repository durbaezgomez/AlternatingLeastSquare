using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace zadanie3
{
    public class DataProvider
    {
        private List<Product> productsFound { get; set; }

        public DataProvider()
        {
            productsFound = new List<Product>();
        }

        // Parsing data from amazon-meta.txt into an array of Product
        public void ParseInitialData(int amountToFind)
        {
            List<List<string>> results = ReadFromFile(amountToFind);
            results = FilterForUnique(results);

            foreach (List<string> result in results)
            {
                string asin = ProcessASIN(result);
                Dictionary<string, int> reviews = ProcessReviews(result);
                Product prod = new Product(asin, reviews);
                productsFound.Add(prod);
            }

            //foreach (Product prod in productsFound)
            //{
            //    Console.Write(prod);
            //}
            
        }

        private List<List<string>> ReadFromFile(int amountToFind)
        {
            List<List<string>> results = new List<List<string>>();
            List<string> current = null;
            string line;

            try
            {
                string pathToFile = "/Users/admin/Desktop/STUDIA/UG/SEM5/ALG/zadanie3/amazon-meta.txt";
                StreamReader file = File.OpenText(pathToFile);

                while ((line = file.ReadLine()) != null && results.Count != amountToFind)
                {
                    if (line.Contains("ASIN:") && current == null)
                        current = new List<string>();
                    else if (line.Length == 0 && current != null)
                    {
                        if (CheckViability(current))
                            results.Add(current);
                        current = null;
                    }
                    if (current != null)
                        current.Add(line);
                }
                //Console.WriteLine("FOUND: " + results.Count);
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return results;
        }

        private bool CheckViability(List<string> product)
        { 
            if (product.Contains("  group: Book"))
            {
                int reviewCount = product.Count - product.FindIndex(x => x.Contains("reviews")) - 1;
                return reviewCount >= 6;
            }
            return false;
        }

        private List<List<string>> FilterForUnique(List<List<string>> list)
        {
            return list
                .GroupBy(x => x.First())
                .Select(x => x.Last())
                .ToList();
        }

        private string ProcessASIN(List<string> data)
        {
            return data.First().Substring(6);
        }

        private Dictionary<string, int> ProcessReviews(List<string> data)
        {
            Dictionary<string, int> reviews = new Dictionary<string, int>();

            var startIndex = data.FindIndex(x => x.Contains("reviews"));
            data.RemoveRange(0, startIndex);

            string customerId;
            int rating;
            foreach(string line in data)
            {
                Match match = Regex.Match(line, @" A[A-Z0-9]+");
                if (match.Success)
                {
                    customerId = match.Value.Substring(1);
                    Match match2 = Regex.Match(line, @"rating: [0-9]");
                    if (match2.Success)
                    {
                        rating = Int32.Parse(match2.Value.Substring(8));
                        try
                        {
                            reviews.Add(customerId, rating);
                        }
                        catch (Exception) // Key already in dictionary (user has previously reviewed the product)
                        {
                            reviews.Remove(customerId);
                            reviews.Add(customerId, rating);
                        }
                    }
                }
            }
            return reviews;
        }
    }
}
