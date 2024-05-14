// using System.Linq.Expressions;

// namespace Cornershop.Service.Infrastructure.Interfaces
// {
//     public interface IUserRepository<User>
//     {
//         Task<User?> GetAsync(Expression<Func<User, bool>> predicate);
//         Task<User?> GetByIdAsync(string id);
//         Task<ICollection<User>> GetListAsync(Expression<Func<User, bool>> predicate,Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null);
//         Task<User> AddAsync(User entity);
//         bool Update(User entity);
//         bool Delete(User entity);
//         bool DeleteById(string id);
//         Task<bool> CheckIfAnyAsync(Expression<Func<User, bool>> predicate);
//         Task<bool> CheckIfAllAsync(Expression<Func<User, bool>> predicate);
//     }
// }