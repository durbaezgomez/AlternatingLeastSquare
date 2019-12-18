using System;
using System.Collections.Generic;
using System.Linq;

namespace zadanie3
{
    public class MatrixProvider
    {
        private readonly DataProvider dataProvider = new DataProvider(amountToFind: 7); // TODO 3 rozmiary list do przeliczenia
        public int[,] RatingsMatrix { get; set; } // out [33]
        public float[,] MatrixU { get; set; } // out [36]
        public float[,] MatrixP { get; set; } // out [35]

        public MatrixProvider()
        {
            RatingsMatrix = ProvideRatingsTable(dataProvider.ResultsList);
            MatrixP = PopulateMatrix(3, 10);
            MatrixU = PopulateMatrix(3, 3);
            Utility<float>.PrintMatrix(MatrixP);
            Utility<float>.PrintMatrix(MatrixU);
        }

        private int[,] ProvideRatingsTable(List<Result> pivotTable)
        {
            var rows = pivotTable.Max(x => x.UserId) + 1;
            var cols = pivotTable.Max(x => x.ProductId) + 1;
            int[,] ratingsMatrix = new int[rows, cols];

            foreach (var result in pivotTable)
            {
                try
                {
                    ratingsMatrix[result.UserId, result.ProductId] = result.Rating;
                }

                catch
                {
                    ratingsMatrix[result.UserId, result.ProductId] = 0;
                    throw new Exception("Rating not found, inserting 0...");
                }

            }

            return ratingsMatrix;
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
