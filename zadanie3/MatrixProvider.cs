using System;
using System.Collections.Generic;

namespace zadanie3
{
    public class MatrixProvider
    {
        private Parser DataProvider { get; }

        private Dictionary<string, int> _customerIds = new Dictionary<string, int>(); // holds original string IDs and converted ones
        private Dictionary<int, int> _productIds = new Dictionary<int, int>(); // holds original string IDs and converted ones
        private int _nextInt = 0;
        private int _nextProdInt = 0;

        private int ProdAmount { get; set; }
        private int UserAmount { get; set; }

        public int[,] RatingsMatrix { get; set; }

        public MatrixProvider(int prodAmount, int userAmount)
        {
            ProdAmount = prodAmount;
            UserAmount = userAmount;

            DataProvider = new Parser(ProdAmount, UserAmount);

            RatingsMatrix = ConvertResultsTableToPivot(DataProvider.ResultsList);

        }

        public int[,] GetCroppedDataFromDataProvider(int prodAmount, int userAmount)
        {
            ProdAmount = prodAmount;
            UserAmount = userAmount;
            var resultsList = DataProvider.GetCroppedData(prodAmount, userAmount);
            var newResultsArray = ConvertResultsTableToPivot(resultsList);
            return newResultsArray;
        }

        private double[,] PopulateMatrix(int d, int xDimension)
        {
            var array = new double[d, xDimension];
            var rnd = new Random();
            for (var i = 0; i < d; i++)
            {
                for (var j = 0; j < xDimension; j++)
                {
                    array[i, j] = (double)rnd.NextDouble();
                }
            }

            return array;
        }

        public double[,] CreateMatrixU(int dim0, int dim1)
        {
            var matrixU = PopulateMatrix(dim0, dim1);
            return matrixU;
        }

        public double[,] CreateMatrixP(int dim0, int dim1)
        {
            var matrixP = PopulateMatrix(dim0, dim1);
            return matrixP;
        }

        private int GetCustomerId(string idToCheck)
        {

            if (_customerIds.TryAdd(idToCheck, _nextInt))
                _nextInt += 1;

            return _customerIds[idToCheck];
        }

        private int GetProductId(int idToCheck)
        {
            if (_productIds.TryAdd(idToCheck, _nextProdInt))
                _nextProdInt += 1;

            return _productIds[idToCheck];
        }

        private int[,] ConvertResultsTableToPivot(List<Result> resultsList)
        {
            var pivotTable = new int[UserAmount, ProdAmount];

            foreach (var result in resultsList)
            {
                pivotTable[GetCustomerId(result.UserId), GetProductId(result.ProductId)] = result.Rating;
            }

            return pivotTable;
        }

        private void PrintRatingsMatrix()
        {
            for (var i = 0; i < RatingsMatrix.GetLength(0); i++)
            {
                for (var j = 0; j < RatingsMatrix.GetLength(1); j++)
                {
                    Console.Write(RatingsMatrix[i, j] + "  ");
                }
                Console.WriteLine();
            }
        }

        public void CleanDictionaries()
        {
            _customerIds = new Dictionary<string, int>();
            _productIds = new Dictionary<int, int>();
            _nextInt = 0;
            _nextProdInt = 0;
        }

    }
}
