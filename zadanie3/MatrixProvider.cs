using System;
using System.Collections.Generic;
using System.Linq;

namespace zadanie3
{
    public class MatrixProvider
    {
        private readonly DataProvider dataProvider = new DataProvider(amountToFind: 7); // TODO 3 rozmiary list do przeliczenia
        public int[,] RatingsMatrix { get; set; }

        public MatrixProvider()
        {
            RatingsMatrix = ProvideRatingsTable(dataProvider.ResultsList);
            PrintRatingsMatrix();
        }

        private void PrintRatingsMatrix()
        {
            for (int i = 0; i < RatingsMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < RatingsMatrix.GetLength(1); j++)
                {
                    Console.Write(RatingsMatrix[i, j] + "   ");
                }

                Console.WriteLine();
            }
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

    }
}
