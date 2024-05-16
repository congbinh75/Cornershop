using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class UpdateCategoryResponse : BaseResponse
    {
        public CategoryDTO Category { get; set; }
    }
}