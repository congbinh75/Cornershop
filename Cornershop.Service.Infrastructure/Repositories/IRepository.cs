using System.Linq.Expressions;

namespace Cornershop.Service.Repositories
{
    public interface IRepository<T>
    {
        Task<T> Get(Expression<Func<T, bool>> predicate);
        Task<T> GetById(string id);
        Task<ICollection<T>> GetList(Expression<Func<T, bool>> predicate);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> DeleteById(string id);
        Task<bool> CheckIfAny(Expression<Func<T, bool>> predicate);
        Task<bool> CheckIfAll(Expression<Func<T, bool>> predicate);
    }
}