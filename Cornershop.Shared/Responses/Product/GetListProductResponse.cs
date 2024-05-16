using System.Collections.Generic;
using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Requests
{
    public class GetListProductResponse
    {
        public ICollection<ProductDTO> ProductList { get; set; }
    }
}
