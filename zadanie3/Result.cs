namespace zadanie3
{
    public class Result
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }

        public Result(string userId, int productId, int rating)
        {
            UserId = userId;
            ProductId = productId;
            Rating = rating;
        }
    }
}
