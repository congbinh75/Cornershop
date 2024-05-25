using Cornershop.Shared.DTOs;
using Cornershop.Shared.Responses;

namespace Cornershop.Shared.Requests
{
    public class UpdateProductResponse : BaseResponse
    {
        public ProductDTO Product { get; set; }
    }
}
