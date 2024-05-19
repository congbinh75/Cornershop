using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cornershop.Service.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .Property(p => p.TotalPrice)
                .HasColumnType("decimal")
                .HasPrecision(18, 4);
        }
    }
}
