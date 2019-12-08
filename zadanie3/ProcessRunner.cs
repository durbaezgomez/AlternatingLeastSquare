using System;
using System.Collections.Generic;
using System.Linq;

namespace zadanie3
{
    public class ProcessRunner
    {
        private readonly DataProvider dataProvider = new DataProvider(amountToFind: 7); // TODO 3 rozmiary list do przeliczenia
        public List<Product> Products { get; set; }

        public ProcessRunner()
        {
            Products = dataProvider.ProductsFound;
            var pivotTable = ProvidePivotTable();
            //var ratingsTable = ProvideRatingsTable(pivotTable);

            //Console.WriteLine(ratingsTable.ToArray());
        }

        private List<List<int>> ProvidePivotTable()
        {
            var pivotTable = new List<List<int>>();

            foreach (var product in Products)
            {
                foreach (var customerIndex in product.Reviews.Keys)
                {
                    var tempList = new List<int>();
                    tempList.Add(customerIndex);
                    tempList.Add(product.Id);
                    tempList.Add(product.Reviews[customerIndex]);
                    pivotTable.Add(tempList);
                }
            }

            Console.WriteLine("\n\nAFTER\n\n");

            pivotTable.GroupBy(x => x[0]).ToList();

            foreach (var row in pivotTable)
            {
                foreach (var item in row)
                {
                    Console.Write(item + " ");
                }
                Console.WriteLine();
            }
            return pivotTable;
        }

        //private List<List<int>> ProvideRatingsTable(List<List<int>> pivotTable)
        //{
        //    var rows = pivotTable.Last()[0];
        //    var cols = pivotTable.;
        //    //int[,] ratingsTable = new int[,];


        //    foreach (var user in pivotTable.Select(x => x.First()))
        //    {
        //        //ratingsTable[user][pivotTable[user][1]] = pivotTable[user][2];
        //    }
        //    return new List<List<int>>();
        //}

    }
}
