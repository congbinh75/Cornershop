using Cornershop.Shared.DTOs;
using Cornershop.Shared.Responses;

namespace Cornershop.Shared.Requests
{
    public class GetProductResponse : BaseResponse
    {
        public ProductDTO Product { get; set; }
    }
}
