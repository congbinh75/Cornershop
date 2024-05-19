using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cornershop.Service.Infrastructure.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder
                .Property(p => p.Price)
                .HasColumnType("decimal")
                .HasPrecision(18, 4);

            builder
                .Property(p => p.OriginalPrice)
                .HasColumnType("decimal")
                .HasPrecision(18, 4);

            builder
                .Property(p => p.Rating)
                .HasColumnType("decimal")
                .HasPrecision(18, 4);

            builder
                .HasOne(c => c.Subcategory)
                .WithMany(s => s.Products)
                .HasForeignKey(c => c.SubcategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
