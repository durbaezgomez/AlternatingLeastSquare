using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace zadanie3
{
    public class DataProvider
    {
        // Parsing data from amazon-meta.txt into an array of Product
        public static Product[] ParseInitialData(int amountToFind) 
        {
            // TODO: parse data from file by id, then access ratings

            var productsFound = new List<Product>();
           
            List<List<string>> results = new List<List<string>>();
            List<string> current = null;
            string line;

            try
            {
                string pathToFile = "/Users/admin/Desktop/STUDIA/UG/SEM5/ALG/zadanie3/amazon-meta.txt";
                StreamReader file = File.OpenText(pathToFile);

                while ((line = file.ReadLine()) != null && productsFound.Count != amountToFind)
                {

                    if (line.Contains("ASIN:") && current == null)
                        current = new List<string>();
                    else if ((line.Contains("") || line.Length == 1) && current != null)
                    {
                        //if (CheckCategory(current))
                        //    results.Add(current);
                        current = null;
                    }
                    if (current != null)
                        current.Add(line);
                }
                // now results should contain amountToFind * products in string format
                Console.WriteLine("FOUND: " + results.Count);
            }

            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Product[] finalProductsArray = productsFound.ToArray();
            return finalProductsArray;
        }

        private static bool CheckCategory(List<string> product)
        {
            return false;
        }

        private static int[] ParseReviews()
        {
            var reviews = new List<int>();

            int[] reviewsArray = reviews.ToArray();
            return reviewsArray;
        }
    }
}
