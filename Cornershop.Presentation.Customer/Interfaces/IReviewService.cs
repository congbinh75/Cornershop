using Cornershop.Shared.DTOs;

namespace Cornershop.Presentation.Customer.Interfaces
{
    public interface IReviewService
    {
        public Task<(ICollection<ReviewDTO> reviews, int count)> GetAllByProduct(string productId, int page, int pageSize);
        public Task<ReviewDTO?> GetReviewOfProductByCurrentUser(string productId);
        public Task<ReviewDTO?> Add(string productId, int rating, string comment);
        public Task<bool> Remove(string productId);
    }
}