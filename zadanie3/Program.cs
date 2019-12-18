namespace zadanie3
{
    class Program
    {
        static void Main(string[] args)
        {
            var alsObject = new ALS();
            var matrixProvider = new MatrixProvider();

            Utility<int>.PrintMatrix(matrixProvider.RatingsMatrix);

            var matrixU = matrixProvider.MatrixU;
            var matrixP = matrixProvider.MatrixP;

            var matrixU_clone = alsObject.createCloneArray(matrixU);
            var matrixP_clone = alsObject.createCloneArray(matrixP);

            var ratings = matrixProvider.RatingsMatrix;

            var d = 3;
            var reg = 0.1; //lambda

            for (var i = 0; i < 100; i++) // dla 100 powtorzen kroku 3, 4, 5, 6
            {

                for (int u = 0; u < ratings.GetLength(0); u++)
                { // dla kazdego uzytkownika

                    var I_u = alsObject.FlatNoZeroOnRow(u, ratings);

                    var P_I_u = alsObject.TakeIndexValues(I_u, matrixP_clone, d);
                    var toUse = 
                    //var P_I_u_T = toUse.
                    var E_p = alsObject.CreateEye(d);

                    // var A_u = mnozenie macierzy * reg

                    var V_u = alsObject.Count_V_u(I_u, P_I_u, ratings, u);

                    //var solution_on_u = gauss

                    //matrixU_clone = alsObject.SwitchGaussColumn(u, matrixU, solution);
                }

                for (var p = 0; p < ratings.GetLength(1); p++) // dla kazdego produktu
                {
                    var I_p = alsObject.FlatNoZeroOnColumn(p, ratings);
                    var U_I_p = alsObject.TakeIndexValues(I_p, matrixU_clone, d);
                    // var U_I_p_T = transponowanie
                    var E_u = alsObject.CreateEye(d);

                    // var B_u = mnozenie macierzy

                    var W_p = alsObject.Count_W_p(I_p, U_I_p, ratings, p);

                    //var solution_on_p = gauss

                    //matrixP_clone = alsObject.SwitchGaussColumn(p, matrixP, solution_on_p);
                }

            }

        }
    }
}