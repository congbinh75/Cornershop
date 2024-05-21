using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class AddSubcategoryResponse : BaseResponse
    {
        public SubcategoryDTO Subcategory { get; set; }
    }
}