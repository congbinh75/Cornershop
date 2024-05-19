using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IReviewService
    {
        public Task<ICollection<ReviewDTO>> GetByProduct(int page, int pageSize, bool isVisible = false);
        public Task<ReviewDTO?> Add(ReviewDTO productDTO);
        public Task<ReviewDTO?> Update(ReviewDTO productDTO);
        public Task<bool> Remove(string id);
    }
}