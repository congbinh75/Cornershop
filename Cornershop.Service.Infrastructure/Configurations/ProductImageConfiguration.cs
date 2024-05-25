using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cornershop.Service.Infrastructure.Configurations
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder
                .HasOne(c => c.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(c => c.ProductId);
                
        }
    }
}
