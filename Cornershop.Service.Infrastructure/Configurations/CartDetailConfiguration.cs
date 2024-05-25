using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cornershop.Service.Infrastructure.Configurations
{
    public class CartDetailConfiguration : IEntityTypeConfiguration<CartDetail>
    {
        public void Configure(EntityTypeBuilder<CartDetail> builder)
        {
            builder
                .HasOne(c => c.Cart)
                .WithMany(c => c.CartDetails)
                .HasForeignKey(c => c.CartId);
            
            builder
                .HasOne(c => c.Product)
                .WithMany(c => c.CartDetails)
                .HasForeignKey(c => c.ProductId);
        }
    }
}
