using Cornershop.Service.Common;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IReviewService
    {
        public Task<(ICollection<ReviewDTO> reviews, int count)> GetAllByProduct(int page, int pageSize, string productId);
        public Task<Result<ReviewDTO?, string?>> GetByProductAndUser(string productId, string userId);
        public Task<Result<ReviewDTO?, string?>> Add(ReviewDTO productDTO);
        public Task<Result<bool, string?>> Remove(ReviewDTO productDTO);
    }
}