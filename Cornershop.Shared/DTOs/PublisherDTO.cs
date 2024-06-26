using System.Collections.Generic;

namespace Cornershop.Shared.DTOs
{
    public class PublisherDTO : BaseDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ProductDTO> Products { get; set; }
    }
}