using Cornershop.Shared.DTOs;
using Cornershop.Shared.Responses;

namespace Cornershop.Shared.Requests
{
    public class AddProductResponse : BaseResponse
    {
        public ProductDTO Product { get; set; }
    }
}
