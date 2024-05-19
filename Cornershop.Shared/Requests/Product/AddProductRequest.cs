using System.Collections.Generic;

namespace Cornershop.Shared.Requests
{
    public class AddProductRequest
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }

        public string SubcategoryId { get; set; }

        public decimal Price { get; set; }

        public decimal OriginalPrice { get; set; }

        public ICollection<byte[]> UploadedImagesFiles { get; set; }

        public int Width { get; set; }

        public int Length { get; set; }

        public int Height { get; set; }

        public int Pages { get; set; }

        public int Format { get; set; }

        public int Stock { get; set; }

        public int PublishedYear { get; set; }

        public ICollection<string> AuthorsIds { get; set; }

        public string PublisherId { get; set; }

        public bool IsVisible { get; set; }
    }
}
