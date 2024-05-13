using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Service.Repositories;

namespace Cornershop.Service.Infrastructure.UnitOfWorks
{
    public class UnitOfWork(CornershopDbContext context) : IUnitOfWork
    {
        private Dictionary<string, object> _repositories = [];

        public IRepository<T> GetRepository<T>() where T : class
		{
			var type = typeof(T).Name;

			if (_repositories.ContainsKey(type) == false)
			{
				var repositoryInstance = new Repository<T>(context);
				_repositories.Add(type, repositoryInstance);
			}

			return (IRepository<T>)_repositories[type];
		}

        public async Task SaveChanges()
        {
            await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}