using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            // TODO: parse data from file by ASIN, then access ratings

            List<List<string>> results = ReadFromFile(amountToFind);
            results = FilterForUnique(results);
            PrintResults(results);
        }

        private static void PrintResults(List<List<string>> results)
        {
            foreach (List<string> list in results)
            {
                Console.WriteLine(list[0]);
            }
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
                // now results should contain amountToFind * products in string format
                Console.WriteLine("FOUND: " + results.Count);
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

        private int[] ParseReviews()
        {
            var reviews = new List<int>();

            int[] reviewsArray = reviews.ToArray();
            return reviewsArray;
        }
    }
}
