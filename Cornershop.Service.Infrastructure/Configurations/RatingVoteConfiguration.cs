using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cornershop.Service.Infrastructure.Configurations
{
    public class RatingVoteConfiguration : IEntityTypeConfiguration<RatingVote>
    {
        public void Configure(EntityTypeBuilder<RatingVote> builder)
        {
            builder
                .HasKey(cv => new { cv.UserId, cv.ProductId });

            builder
                .HasOne(cv => cv.User)
                .WithMany(u => u.RatingVotes)
                .HasForeignKey(cv => cv.UserId);

            builder
                .HasOne(cv => cv.Product)
                .WithMany(c => c.RatingVotes)
                .HasForeignKey(cv => cv.ProductId);
        }
    }
}