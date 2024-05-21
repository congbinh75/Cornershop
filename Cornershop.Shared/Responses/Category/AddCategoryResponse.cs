using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class AddCategoryResponse : BaseResponse
    {
        public CategoryDTO Category { get; set; }
    }
}