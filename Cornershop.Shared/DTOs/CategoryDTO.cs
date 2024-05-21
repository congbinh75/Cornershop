using System.Collections.Generic;
using System.ComponentModel;

namespace Cornershop.Shared.DTOs

{
    public class CategoryDTO : BaseDTO
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<ProductDTO> Products { get; set; }
    }
}