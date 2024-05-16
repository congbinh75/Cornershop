using System.Collections.Generic;

namespace Cornershop.Shared.Requests
{
    public class AddProductRequest
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string CategoryId { get; set; }

        public decimal Price { get; set; }

        public ICollection<byte[]> UploadedImagesFiles { get; set; }
    }
}
