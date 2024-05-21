using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetCategoryResponse : BaseResponse
    {
        public CategoryDTO Category { get; set; }
    }
}