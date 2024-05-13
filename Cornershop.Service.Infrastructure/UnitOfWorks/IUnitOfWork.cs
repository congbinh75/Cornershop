using Cornershop.Service.Repositories;

namespace Cornershop.Service.Infrastructure.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : class;
        Task SaveChanges();
        void Dispose();
    }
}