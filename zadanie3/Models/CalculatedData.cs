namespace zadanie3
{
    public class CalculatedData
    {
        public double Reg { get; set; }
        public double Dimension { get; set; }
        public double SummingError { get; set; }
        public double SquareError { get; set; }

        public CalculatedData(double reg, double dimension, double summingError, double squareError)
        {
            Reg = reg;
            Dimension = dimension;
            SummingError = summingError;
            SquareError = squareError;
        }
    }
}
