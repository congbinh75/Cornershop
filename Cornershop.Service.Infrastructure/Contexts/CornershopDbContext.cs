using Cornershop.Service.Common;
using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Contexts
{
    public class CornershopDbContext(DbContextOptions<CornershopDbContext> options, ITokenInfoProvider tokenInfoProvider) : DbContext(options)
    {
        private readonly ITokenInfoProvider tokenInfoProvider = tokenInfoProvider;

        public DbSet<Author> Authors { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartDetail> CartDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Order> Orders{ get; set; }
        public DbSet<OrderDetail> OrdersDetails { get; set;}
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var now = DateTimeOffset.UtcNow;
            var currentUser = await Users.FirstOrDefaultAsync(u => u.Id == tokenInfoProvider.Id, cancellationToken: cancellationToken);

            var changedEntities = ChangeTracker.Entries();
            foreach (var changedEntity in changedEntities)
            {
                if (changedEntity.Entity is BaseEntity entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreatedOn = now;
                            entity.UpdatedOn = now;
                            entity.CreatedBy = currentUser;
                            entity.UpdatedBy = currentUser;
                            break;
                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                            Entry(entity).Property(x => x.CreatedOn).IsModified = false;
                            entity.UpdatedOn = now;
                            entity.UpdatedBy = currentUser;
                            break;
                    }
                }
                else if (changedEntity.Entity is User user)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            user.CreatedOn = now;
                            user.UpdatedOn = now;
                            break;
                        case EntityState.Modified:
                            Entry(user).Property(x => x.CreatedOn).IsModified = false;
                            user.UpdatedOn = now;
                            break;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}