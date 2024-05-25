using System.Collections.Generic;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Responses;

namespace Cornershop.Shared.Requests
{
    public class GetAllProductResponse : BaseResponse
    {
        public ICollection<ProductDTO> Products { get; set; }

        public int PagesCount { get; set; }
    }
}
