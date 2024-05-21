using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetSubcategoryResponse : BaseResponse
    {
        public SubcategoryDTO Subcategory { get; set; }
    }
}