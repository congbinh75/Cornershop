namespace Cornershop.Shared.Requests
{
    public class AddReviewRequest
    {
        public string ProductId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}
