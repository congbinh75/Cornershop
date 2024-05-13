using Cornershop.Service.Common;
using Cornershop.Service.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Contexts
{
    public class CornershopDbContext(DbContextOptions<DbContext> options, ITokenInfoProvider tokenInfoProvider) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("cornershop");
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
                            entity.CreatedTime = now;
                            entity.UpdatedTime = now;
                            entity.CreatedBy = currentUser;
                            entity.CreatedById = currentUser?.Id ?? "";
                            entity.UpdatedBy = currentUser;
                            entity.UpdatedById = currentUser?.Id ?? "";
                            break;
                        case EntityState.Modified:
                            Entry(entity).Property(x => x.CreatedBy).IsModified = false;
                            Entry(entity).Property(x => x.CreatedById).IsModified = false;
                            Entry(entity).Property(x => x.CreatedTime).IsModified = false;
                            entity.UpdatedTime = now;
                            entity.UpdatedBy = currentUser;
                            entity.UpdatedById = currentUser?.Id ?? "";
                            break;
                    }
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}