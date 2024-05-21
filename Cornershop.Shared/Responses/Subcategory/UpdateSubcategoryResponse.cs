using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class UpdateSubcategoryResponse : BaseResponse
    {
        public SubcategoryDTO Subcategory { get; set; }
    }
}