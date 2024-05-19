using System;
using System.Collections.Generic;

namespace Cornershop.Shared.DTOs
{
    public class ProductDTO : BaseDTO
    {
        public string Name { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        
        public string CategoryId { get; set; }

        public CategoryDTO Category { get; set; }

        public string SubcategoryId { get; set; }

        public SubcategoryDTO Subcategory { get; set; }

        public decimal Price { get; set; }

        public decimal OriginalPrice { get; set; }

        public int Width { get; set; }

        public int Length { get; set; }

        public int Height { get; set; }

        public int Pages { get; set; }

        public int Format { get; set; }

        public int Stock { get; set; }

        public int PublishedYear { get; set; }

        public ICollection<byte[]> UploadImagesFiles { get; set; }

        public ICollection<string> ImagesUrls { get; set; }

        public decimal Rating { get; set; }

        public ICollection<ReviewDTO> Reviews { get; set; }

        public ICollection<string> AuthorsIds { get; set; }

        public ICollection<AuthorDTO> Authors { get; set; }

        public string PublisherId { get; set; }

        public PublisherDTO Publisher { get; set; }

        public bool IsVisible { get; set; }
    }
}