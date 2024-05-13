using System.Linq.Expressions;
using Cornershop.Service.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Repositories
{
    public class Repository<T>(CornershopDbContext context) : IRepository<T> where T : class
    {
        protected CornershopDbContext context = context;
		internal DbSet<T> dbSet = context.Set<T>();

        public Task<T> Get(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<T>> GetList(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<T> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> Update(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckIfAny(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CheckIfAll(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }
    }
}