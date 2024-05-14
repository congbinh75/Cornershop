// using Cornershop.Service.Infrastructure.Contexts;
// using Cornershop.Service.Infrastructure.Interfaces;

// namespace Cornershop.Service.Infrastructure.UnitOfWorks
// {
//     public class UnitOfWork(CornershopDbContext context) : IUnitOfWork
//     {
//         private Dictionary<string, object> _repositories = [];

//         public IUserRepository<T> GetRepository<T>() where T : class
// 		{
// 			var type = typeof(T).Name;

// 			if (_repositories.ContainsKey(type) == false)
// 			{
// 				var repositoryInstance = new UserRepository<T>(context);
// 				_repositories.Add(type, repositoryInstance);
// 			}

// 			return (IUserRepository<T>)_repositories[type];
// 		}

//         public async Task SaveChanges()
//         {
//             await context.SaveChangesAsync();
//         }

//         public void Dispose()
//         {
//             context.Dispose();
//         }
//     }
// }