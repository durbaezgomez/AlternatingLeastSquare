namespace zadanie3
{
    class Program
    {
        static void Main(string[] args)
        {
            DataProvider parser = new DataProvider();
            var products = parser.ParseInitialData(10);
        }
    }
}