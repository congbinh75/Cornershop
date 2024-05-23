using System.Collections.Generic;
using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Requests
{
    public class GetAllProductResponse
    {
        public ICollection<ProductDTO> Products { get; set; }

        public int PagesCount { get; set; }
    }
}
