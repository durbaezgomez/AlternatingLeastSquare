using ServiceStack;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;

namespace zadanie3
{
    public class Program
    {
        public static int Iterations = 20;
        public static List<CalculatedData> SmallData = new List<CalculatedData>();
        public static List<CalculatedData> MediumData = new List<CalculatedData>();
        public static List<CalculatedData> BigData = new List<CalculatedData>();

        public static List<ExecutionTime> DimensionTime = new List<ExecutionTime>();

        public static void Main()
        {
            var prodAmount = 40;
            var userAmount = 20;
            AlsRun(prodAmount, userAmount, new MatrixProvider(prodAmount, userAmount), "small"); // dla malego zbioru

            prodAmount = 100;
            userAmount = 50;
            AlsRun(prodAmount, userAmount, new MatrixProvider(prodAmount, userAmount), "medium"); // dla sredniego zbioru

            prodAmount = 1000;
            userAmount = 500;
            AlsRun(prodAmount, userAmount, new MatrixProvider(prodAmount, userAmount), "big"); // dla duzego zbioru

            SaveDataAsCsv();

            Console.WriteLine("Results saved as CSV");
        }

        public static void SaveDataAsCsv()
        {
            if (!SmallData.IsNullOrEmpty())
            {
                var csv = CsvSerializer.SerializeToCsv(SmallData);
                File.AppendAllText($"SmallData.csv", csv);
            }

            if (!MediumData.IsNullOrEmpty())
            {
                var csv = CsvSerializer.SerializeToCsv(MediumData);
                File.AppendAllText($"MediumData.csv", csv);
            }

            if (!BigData.IsNullOrEmpty())
            {
                var csv = CsvSerializer.SerializeToCsv(BigData);
                File.AppendAllText($"BigData.csv", csv);
            }

            if (!DimensionTime.IsNullOrEmpty())
            {
                var csv = CsvSerializer.SerializeToCsv(DimensionTime);
                File.AppendAllText($"DimensionTime.csv", csv);
            }

        }

        private static void AlsRun(int prodAmount, int userAmount, MatrixProvider matrixProvider, string size)
        {
            var alsObject = new ALS();

            var lambdaArray = new double[] { 0.1, 0.5, 1.0 };
            var dimensionArray = new double[] { 3D, 8D, 13D, 21D };

            var ratingsTest = matrixProvider.GetCroppedDataFromDataProvider(prodAmount, userAmount);
            var ratingsLearning = ratingsTest.Clone() as int[,];

            var pairs = new int[(int)(0.5 * prodAmount), 2];
            var counter = 0;

            var rand = new Random();

            for (var i = 0; i < ratingsLearning.GetLength(0); i++) // zeby wyzerowac kilka elementow w tablicy
            {
                for (var j = 0; j < ratingsLearning.GetLength(1); j++)
                {
                    if (rand.Next(1, 10) > 7 && (ratingsLearning[i, j] != 0) && counter < pairs.GetLength(0))
                    {
                        ratingsLearning[i, j] = 0;
                        pairs[counter, 0] = i;
                        pairs[counter, 1] = j;
                        counter++;
                    }
                }
            }

            foreach (var reg in lambdaArray)
            {
                foreach (var dimension in dimensionArray)
                {
                    var matrixU = matrixProvider.CreateMatrixU((int)dimension, userAmount);
                    var matrixP = matrixProvider.CreateMatrixP((int)dimension, prodAmount);

                    var matrixUClone = matrixU.Clone() as double[,];
                    var matrixPClone = matrixP.Clone() as double[,];

                    var watch = System.Diagnostics.Stopwatch.StartNew();


                    for (var i = 0; i < Iterations; i++)
                    {

                        for (var userIndex = 0; userIndex < ratingsLearning.GetLength(0); userIndex++)
                        {
                            var I_u = alsObject.FlatNoZeroOnRow(userIndex, ratingsLearning);
                            var P_I_u = alsObject.TakeIndexValues(I_u, matrixPClone, dimension);
                            var P_I_u_T = Matrix.Transpose(P_I_u);

                            var E_p = alsObject.CreateEye(dimension);

                            var A_u = Matrix.Summing(Matrix.Multiplication(P_I_u, P_I_u_T), Matrix.Multiplication(E_p, reg));
                            var V_u = alsObject.Count_V_u(I_u, P_I_u_T, ratingsLearning, userIndex);

                            var usingMatrix = new Matrix(A_u, V_u);
                            usingMatrix.CalculatePG(A_u, V_u);
                            var solutionOnU = usingMatrix.VectorXGauss;

                            matrixUClone = alsObject.SwitchGaussColumn(userIndex, matrixUClone, solutionOnU);
                        }

                        for (var productIndex = 0; productIndex < ratingsLearning.GetLength(1); productIndex++)
                        {

                            var I_p = alsObject.FlatNoZeroOnColumn(productIndex, ratingsLearning);
                            var U_I_p = alsObject.TakeIndexValues(I_p, matrixUClone, dimension);
                            var U_I_p_T = Matrix.Transpose(U_I_p);

                            var E_u = alsObject.CreateEye(dimension);

                            var B_u = Matrix.Summing(Matrix.Multiplication(U_I_p, U_I_p_T), Matrix.Multiplication(E_u, reg));

                            var W_p = alsObject.Count_W_p(I_p, U_I_p_T, ratingsLearning, productIndex);

                            var usingMatrix = new Matrix(B_u, W_p);

                            usingMatrix.CalculatePG(B_u, W_p);
                            var solutionOnP = usingMatrix.VectorXGauss;

                            matrixPClone = alsObject.SwitchGaussColumn(productIndex, matrixPClone, solutionOnP);
                        }

                        var matrixUCloneT = Matrix.Transpose(matrixUClone);

                        var finalResult = Matrix.Multiplication(matrixUCloneT, matrixPClone);
                        //Console.WriteLine("For lambda = " + reg + " and d = " + dimension);
                        //Utility.PrintMatrixComparison(ratingsTest, finalResult); 
                        //Utility.PrintComparisonOnZeros(ratingsTest, ratingsLearning, finalResult, pairs);

                        var errors = Utility.CountError(ratingsTest, finalResult, pairs);
                        var result = new CalculatedData(reg, dimension, errors.Item1, errors.Item2);

                        switch (size)
                        {
                            case "small":
                                SmallData.Add(result);
                                break;

                            case "medium":
                                MediumData.Add(result);
                                break;

                            case "big":
                                BigData.Add(result);
                                break;

                            default:
                                throw new Exception("This size does not exist.");
                                break;

                        }

                    }
                    watch.Stop();
                    var elapsedTime = watch.ElapsedMilliseconds;

                    if (size == "big" && reg == 0.1)
                    {
                        DimensionTime.Add(new ExecutionTime(dimension, elapsedTime));
                    }

                }

            }
            matrixProvider.CleanDictionaries();

        }

    }
}