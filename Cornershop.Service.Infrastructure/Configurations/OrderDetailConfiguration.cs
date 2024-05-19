using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cornershop.Service.Infrastructure.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder
                .HasOne(c => c.Product)
                .WithMany(u => u.OrderDetails)
                .HasForeignKey(c => c.ProductId);

            builder
                .HasOne(c => c.Order)
                .WithMany(u => u.OrderDetails)
                .HasForeignKey(c => c.OrderId);

            builder
                .Property(p => p.Price)
                .HasColumnType("decimal")
                .HasPrecision(18, 4);
        }
    }
}
