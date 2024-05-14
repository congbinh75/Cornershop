// using System.Linq.Expressions;
// using Cornershop.Service.Infrastructure.Contexts;
// using Cornershop.Service.Infrastructure.Entities;
// using Cornershop.Service.Infrastructure.Interfaces;
// using Microsoft.EntityFrameworkCore;

// namespace Cornershop.Service.Infrastructure.Repositories
// {
//     public class UserRepository<T>(CornershopDbContext context) : IUserRepository<User>
//     {
//         protected CornershopDbContext context = context;
// 		internal DbSet<User> dbSet = context.Set<User>();

//         public async Task<User?> GetAsync(Expression<Func<User, bool>> expression)
//         {
//             return await dbSet.Where(expression).FirstOrDefaultAsync();
//         }

//         public async Task<User?> GetByIdAsync(string id)
//         {
//             return await dbSet.FindAsync(id);
//         }

//         public async Task<ICollection<User>> GetListAsync(Expression<Func<User, bool>> expression, Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null)
//         {
//             IQueryable<User> query = dbSet;
//             if (expression != null) _ = query.Where(expression);
//             if (orderBy != null) return await orderBy(query).ToListAsync();
//             return await query.ToListAsync();
//         }

//         public async Task<User> AddAsync(User entity)
//         {
//             await dbSet.AddAsync(entity);
//             return entity;
//         }

//         public bool Update(User entity)
//         {
//             context.Entry(entity).State = EntityState.Modified;
//             return true;
//         }

//         public bool Delete(User entity)
//         {
//             dbSet.Remove(entity);
//             return true;
//         }

//         public bool DeleteById(string id)
//         {
//             var entity = dbSet.Find(id);
//             dbSet.Remove(entity);
//             return true;
//         }

//         public async Task<bool> CheckIfAnyAsync(Expression<Func<User, bool>> predicate)
//         {
//             return await dbSet.AnyAsync(predicate);
//         }

//         public async Task<bool> CheckIfAllAsync(Expression<Func<User, bool>> predicate)
//         {
//             return await dbSet.AllAsync(predicate);
//         }
//     }
// }