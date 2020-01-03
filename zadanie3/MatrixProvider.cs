using System;
using System.Collections.Generic;

namespace zadanie3
{
    public class MatrixProvider
    {
        private Parser dataProvider { get; }

        private readonly Dictionary<string, int> _customerIds = new Dictionary<string, int>(); // holds original string IDs and converted ones
        private readonly Dictionary<int, int> _productIds = new Dictionary<int, int>(); // holds original string IDs and converted ones
        private int nextInt = 0;
        private int nextProdInt = 0;

        private int ProdAmount { get; set; }
        private int UserAmount { get; set; }

        public int[,] RatingsMatrix { get; set; }


        public int[,] GetCroppedDataFromDataProvider(int prodAmount, int userAmount)
        {
            ProdAmount = prodAmount;
            UserAmount = userAmount;
            var ResultsList = dataProvider.GetCroppedData(prodAmount, userAmount);
            var newResultsArray = ConvertResultsTableToPivot(ResultsList);
            return newResultsArray;
        }

        //public void setRatingsMatrixWithAmounts( int prodAmount, int userAmount)
        //{
        //    ProdAmount = prodAmount;
        //    UserAmount = userAmount;
        //    RatingsMatrix = ConvertResultsTableToPivot(dataProvider.ResultsList);
        //}

        public float[,] CreateMatrixU (int dim_0, int dim_1)
        {
            var MatrixU = PopulateMatrix(dim_0, dim_1);
            return MatrixU;
        }

        public float[,] CreateMatrixP(int dim_0, int dim_1)
        {
            var MatrixP = PopulateMatrix(dim_0, dim_1);
            return MatrixP;
        }

        public MatrixProvider(int prodAmount, int userAmount)
        {
            ProdAmount = prodAmount;
            UserAmount = userAmount;

            dataProvider = new Parser(ProdAmount, UserAmount);

            RatingsMatrix = ConvertResultsTableToPivot(dataProvider.ResultsList);

        }

        private int GetCustomerId(string idToCheck)
        {

            if (_customerIds.TryAdd(idToCheck, nextInt))
                nextInt += 1;

            return _customerIds[idToCheck];
        }

        private int GetProductId(int idToCheck)
        {
            if(_productIds.TryAdd(idToCheck, nextProdInt))
                nextProdInt += 1;

            return _productIds[idToCheck];
        }

        private int[,] ConvertResultsTableToPivot(List<Result> resultsList)
        {
            int[,] pivotTable = new int[UserAmount, ProdAmount];

            foreach (var result in resultsList)
            {
                pivotTable[GetCustomerId(result.UserId), GetProductId(result.ProductId)] = result.Rating;
            }

            return pivotTable;
        }

        private void PrintRatingsMatrix()
        {
            for (int i = 0; i < RatingsMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < RatingsMatrix.GetLength(1); j++)
                {
                    Console.Write(RatingsMatrix[i, j] + "  ");
                }
                Console.WriteLine();
            }
        }

        private float[,] PopulateMatrix(int d, int xDimension)
        {
            float[,] array = new float[d, xDimension];
            Random rnd = new Random();
            for (int i = 0; i < d; i++)
            {
                for (int j = 0; j < xDimension; j++)
                {
                    array[i, j] = (float)rnd.NextDouble();
                }
            }

            return array;
        }

    }
}
