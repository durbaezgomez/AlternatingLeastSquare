using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace zadanie3
{
    public class DataProvider
    {
        public List<Result> ResultsList = new List<Result>();
        private readonly Dictionary<string, int> _customerIds = new Dictionary<string, int>();
        private int _nextInt = 0;

        public DataProvider(int amountToFind)
        {
            ResultsList = ParseInitialData(amountToFind);
        }

        public List<Result> ParseInitialData(int amountToFind)
        {
            var productsFound = new List<Product>();
            List<List<string>> results = ReadFromFile(amountToFind);
            results = FilterForUnique(results);
            int id = 0;
            foreach (List<string> result in results)
            {
                Dictionary<int, int> reviews = ProcessReviews(result);
                Product prod = new Product(id, reviews);
                productsFound.Add(prod);
                id += 1;
            }

            foreach (var product in productsFound)
            {
                foreach (var customerIndex in product.Reviews.Keys)
                {
                    var result = new Result(customerIndex, product.Id, product.Reviews[customerIndex]);
                    ResultsList.Add(result);
                }
            }

            return ResultsList;
        }

        private List<List<string>> ReadFromFile(int amountToFind)
        {
            List<List<string>> results = new List<List<string>>();
            List<string> current = null;
            string line;
            try
            {
                string pathToFile = "zadanie3.amazon-meta.txt";
                //string pathToFile = "..\\AlternatingLeastSquare\\zadanie3\\amazon-meta.txt";//windows-styled path
                //string pathToFile = "//AlternatingLeastSquare//zadanie3//amazon-meta.txt"; //unix-styled path

                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(pathToFile))
                {
                    using (var file = new StreamReader(stream))
                    {
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
                    }

                }
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

        private Dictionary<int, int> ProcessReviews(List<string> data)
        {
            Dictionary<string, int> reviews = new Dictionary<string, int>();

            var startIndex = data.FindIndex(x => x.Contains("reviews"));
            data.RemoveRange(0, startIndex);

            string customerId;
            int rating;
            foreach (string line in data)
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

            Dictionary<int, int> reviewsConverted = new Dictionary<int, int>();

            foreach (var customer in reviews.Keys)
            {
                if (_customerIds.Keys.Contains(customer))
                {
                    reviewsConverted.Add(_customerIds[customer], reviews[customer]);
                }
                else
                {
                    _customerIds.Add(customer, _nextInt);
                    reviewsConverted.Add(_nextInt, reviews[customer]);
                    _nextInt += 1;
                }
            }

            return reviewsConverted;
        }
    }
}
