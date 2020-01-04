using System;

namespace zadanie3
    
{
    class Program
    {
        public static float [,,] small_data = new float [10, 12, 3]; // rozmiar to  [ ilosc_iteracji , ilosc_lambd * ilosc_d  , 3 ]  , a to 3 = lambda, dimension i blad
        public static float [,,] medium_data = new float [10, 12, 3];
        public static float [,,] big_data = new float [10, 12, 3];

        public static float [,] dimension_time = new float[4, 2]; // [ 4 rozne dimensiony, 2 ] , a to 2 = dimension konkretny i czas dzialania. Liczymy dla big_data i dla lambda = 0.1

        static void Main(string[] args)
        {

            int prodAmount = 10;
            int userAmount = 5;

            var matrixProvider = new MatrixProvider(prodAmount, userAmount); // RatingsMatrix ustalony przy inicjalizacji na 40x20

            AlsRun(prodAmount, userAmount, matrixProvider, "small"); // dla malego zbioru

            prodAmount = 100;
            userAmount = 50;
            AlsRun(prodAmount, userAmount, matrixProvider, "medium"); // dla sredniego zbioru

            prodAmount = 1000;
            userAmount = 500;
            AlsRun(prodAmount, userAmount, matrixProvider, "big"); // dla duzego zbioru



        }



        private static void AlsRun(int prodAmount, int userAmount, MatrixProvider matrixProvider, string size)
        {

            var alsObject = new ALS();

            var lambdaArray = new float[] { 0.1F, 0.5F, 1.0F };
            var dimensionArray = new float[] { 3, 8, 13, 21};

            var ratings_test = matrixProvider.GetCroppedDataFromDataProvider(prodAmount, userAmount);
            var ratings_learning = ratings_test.Clone() as int[,];

            var pairs = new int[(int)(0.5 * prodAmount), 2];
            int counter = 0;

            var rand = new Random();

            for (int i = 0; i < ratings_learning.GetLength(0); i++) // zeby wyzerowac kilka elementow w tablicy
            {
                for (int j = 0; j < ratings_learning.GetLength(1); j++)
                {
                    if ( rand.Next(1, 10) > 7 && (ratings_learning[i, j] != 0) && counter < pairs.GetLength(0)) 
                    {
                        ratings_learning[i, j] = 0;
                        pairs[counter, 0] = i;
                        pairs[counter, 1] = j;
                        counter++;
                    }
                }
            }

            int data_counter = 0; 



            foreach (var reg in lambdaArray)
            {
                var dimension_time_counter = 0;
                foreach (var dimension in dimensionArray)
                {
                    var matrixU = matrixProvider.CreateMatrixU((int)dimension, userAmount);
                    var matrixP = matrixProvider.CreateMatrixP((int)dimension, prodAmount);

                    var matrixU_clone = matrixU.Clone() as float[,];
                    var matrixP_clone = matrixP.Clone() as float[,];

                    var watch = System.Diagnostics.Stopwatch.StartNew();


                    for (var i = 0; i < 10; i++)
                    {

                        for (int userIndex = 0; userIndex < ratings_learning.GetLength(0); userIndex++)
                        {
                            var I_u = alsObject.FlatNoZeroOnRow(userIndex, ratings_learning);
                            var P_I_u = alsObject.TakeIndexValues(I_u, matrixP_clone, dimension);
                            var P_I_u_T = Matrix.Transpose(P_I_u);

                            var E_p = alsObject.CreateEye(dimension);

                            var A_u = Matrix.Summing(Matrix.Multiplication(P_I_u, P_I_u_T), Matrix.Multiplication(E_p, reg));
                            var V_u = alsObject.Count_V_u(I_u, P_I_u_T, ratings_learning, userIndex);

                            var usingMatrix = new Matrix(A_u, V_u);
                            usingMatrix.CalculatePG(A_u, V_u);
                            var solutionOnU = usingMatrix.VectorXGauss;

                            matrixU_clone = alsObject.SwitchGaussColumn(userIndex, matrixU_clone, solutionOnU);
                        }

                        for (var productIndex = 0; productIndex < ratings_learning.GetLength(1); productIndex++)
                        {

                            var I_p = alsObject.FlatNoZeroOnColumn(productIndex, ratings_learning);
                            var U_I_p = alsObject.TakeIndexValues(I_p, matrixU_clone, dimension);
                            var U_I_p_T = Matrix.Transpose(U_I_p);

                            var E_u = alsObject.CreateEye(dimension);

                            var B_u = Matrix.Summing(Matrix.Multiplication(U_I_p, U_I_p_T), Matrix.Multiplication(E_u, reg));

                            var W_p = alsObject.Count_W_p(I_p, U_I_p_T, ratings_learning, productIndex);

                            var usingMatrix = new Matrix(B_u, W_p);

                            usingMatrix.CalculatePG(B_u, W_p);
                            var solutionOnP = usingMatrix.VectorXGauss;

                            matrixP_clone = alsObject.SwitchGaussColumn(productIndex, matrixP_clone, solutionOnP);
                        }

                        var matrixU_clone_T = Matrix.Transpose(matrixU_clone);

                        var final_result = Matrix.Multiplication(matrixU_clone_T, matrixP_clone);
                        Console.WriteLine("For lambda = " + reg + " and d = " + dimension);
                        //Utility<float>.PrintMatrixComparison(ratings_test, final_result);
                        //Utility<float>.PrintComparisonOnZeros(ratings_test, ratings_learning, final_result, pairs);


                        switch (size)
                        {
                            case "small":
                                small_data[i, data_counter, 0] = reg;
                                small_data[i, data_counter, 1] = dimension;
                                small_data[i, data_counter, 2] = Utility<float>.CountError(ratings_test, final_result, pairs);
                                //small_data[i, data_counter, 3] = elapsed_time;

                                break;
                            case "medium":
                                medium_data[i, data_counter, 0] = reg;
                                medium_data[i, data_counter, 1] = dimension;
                                medium_data[i, data_counter, 2] = Utility<float>.CountError(ratings_test, final_result, pairs);
                                //medium_data[i, data_counter, 3] = elapsed_time;
                                break;
                            case "big":
                                big_data[i, data_counter, 0] = reg;
                                big_data[i, data_counter, 1] = dimension;
                                big_data[i, data_counter, 2] = Utility<float>.CountError(ratings_test, final_result, pairs);
                                //big_data[i, data_counter, 3] = elapsed_time;
                                break;
                            default:
                                Console.WriteLine("This size does not exist.");
                                break;

                        }

                    }                 
                    data_counter++;
                    watch.Stop();
                    var elapsed_time = watch.ElapsedMilliseconds;

                    if(size == "big" && reg == 0.1F)
                    {
                        dimension_time[dimension_time_counter, 0] = dimension;
                        dimension_time[dimension_time_counter, 1] = elapsed_time;
                    }

                    dimension_time_counter++;
                }
                
            }

            switch (size)
            {
                case "small":
                    Utility<float>.PrintData(small_data);
                    
                    break;
                case "medium":
                    Utility<float>.PrintData(medium_data);
                    

                    break;
                case "big":
                    Utility<float>.PrintData(big_data);
                    Utility<float>.PrintTime(dimension_time);

                    break;
                default:
                    Console.WriteLine("This size does not exist.");
                    break;
            }

            matrixProvider.cleanDictionaries();

        }

        //private static int[,] randomZeros(int [,] array)
        //{
        //    for (int i = 0; i < array.GetLength(0); i++)
        //    {
        //        for (int j = 0; j < array.GetLength(1); j++)
        //        {
        //            Console.Write(array[i, j] + "   ");
        //        }
        //        Console.WriteLine();
        //    }
        //    return array;
        //}
    }
}