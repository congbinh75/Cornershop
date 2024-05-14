namespace Cornershop.Service.Infrastructure.Entities
{
    public class RatingVote
    {
        public required Product Product { get; set; }

        public required string ProductId { get; set; }

        public required User User { get; set; }

        public required string UserId { get; set; }

        public required int Rate { get; set; }
    }
}