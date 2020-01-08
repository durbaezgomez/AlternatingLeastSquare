using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace zadanie3
{
    public class Parser
    {
        public List<Result> ParsedData = new List<Result>();
        public List<Result> ResultsList = new List<Result>();

        private int ProdAmount { get; set; }
        private int UserAmount { get; set; }

        public Stopwatch Watch { get; private set; }

        public Parser(int prodAmount, int userAmount)
        {
            ProdAmount = prodAmount;
            UserAmount = userAmount;
            Watch = Stopwatch.StartNew();
            ParsedData = ParseInitialData();

            ResultsList = GetCroppedData(ProdAmount, UserAmount);
        }

        public List<Result> ParseInitialData()
        {
            var parsingTime = Watch.Elapsed;
            ReadFromFile();
            //Console.WriteLine("PARSED The FILE IN " + (Watch.Elapsed - parsingTime));
            //Console.WriteLine("ERROR RATIO: " + ResultsList.Count() / ((double)ProdAmount * UserAmount));

            return ResultsList;
        }

        private void ReadFromFile()
        {
            List<string> current = null;
            var reviews = new List<Dictionary<string, int>>();

            string line;
            string customerId;
            int rating;
            int productId = 0;

            var parsingTime = Watch.Elapsed;
            var prodTime = Watch.Elapsed;

            try
            {
                const string pathToFile = "zadanie3.amazon-meta.txt";

                using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(pathToFile);
                using var file = new StreamReader(stream);
                while ((line = file.ReadLine()) != null)
                {
                    if (line.Contains("ASIN:") && current == null)
                    {
                        current = new List<string>();
                        parsingTime = Watch.Elapsed;
                    }
                    else if (line.Length == 0 && current != null)
                    {
                        if (CheckViability(current))
                        {
                            reviews.Add(ProcessReviews(current));

                            foreach (var review in reviews[productId])
                            {
                                customerId = review.Key;
                                rating = review.Value;
                                var result = new Result(customerId, productId, rating);
                                ResultsList.Add(result);
                            }
                            productId += 1;
                            //Console.WriteLine("PARSED THE PRODUCT IN " + (Watch.Elapsed - parsingTime));
                        }
                        current = null;
                    }

                    if (current != null)
                        current.Add(line);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<Result> GetCroppedData(int prodAmount, int userAmount)
        {
            TimeSpan croppingTime = Watch.Elapsed;
            Console.WriteLine("Cropping aggregated data...");

            var mostRatedProducts = ParsedData
                .GroupBy(x => x.ProductId)
                .Select(group => new
                {
                    ProductId = group.Key,
                    RatingCount = group.Count()
                })
                .OrderByDescending(x => x.RatingCount)
                .Take(prodAmount)
                .Select(x => x.ProductId)
                .ToList();

            var byProductResults = ParsedData
                .Where(x => mostRatedProducts.Contains(x.ProductId)).ToList();

            var mostActiveUsers = byProductResults
                .GroupBy(x => x.UserId)
                .Select(group => new
                {
                    UserId = group.Key,
                    Count = group.Count()
                })
                .OrderByDescending(x => x.Count)
                .Where(x => x.Count < 500)
                .Take(userAmount)
                .Select(x => x.UserId)
                .ToList();

            var filteredResults = byProductResults
                .Where(x => mostActiveUsers.Contains(x.UserId))
                .ToList();

            Console.WriteLine("CROPPED THE PRODUCT IN " + (Watch.Elapsed - croppingTime));

            return filteredResults;
        }

        private bool CheckViability(List<string> product)
        {
            if (product.Contains("  group: Book"))
            {
                var reviewCount = product.Count - product.FindIndex(x => x.Contains("reviews")) - 1;
                return reviewCount >= UserAmount / 2;
            }

            return false;
        }

        private Dictionary<string, int> ProcessReviews(List<string> data)
        {
            var reviews = new Dictionary<string, int>();

            var startIndex = data.FindIndex(x => x.Contains("reviews"));
            data.RemoveRange(0, startIndex);

            string customerId;
            int rating;
            foreach (string line in data)
            {
                var match = Regex.Match(line, @" A[A-Z0-9]+");
                if (match.Success)
                {
                    customerId = match.Value.Substring(1);
                    var match2 = Regex.Match(line, @"rating: [0-9]");
                    if (match2.Success)
                    {
                        rating = int.Parse(match2.Value.Substring(8));
                        if (reviews.Keys.Contains(customerId))
                            reviews.Remove(customerId);
                        reviews.Add(customerId, rating);
                    }
                }
            }

            return reviews;
        }
    }
}
