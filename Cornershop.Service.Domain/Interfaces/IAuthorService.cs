using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IAuthorService
    {
        public Task<AuthorDTO?> GetById(string id);
        public Task<ICollection<AuthorDTO>> GetAll(int page, int pageSize);
        public Task<int> GetCount();
        public Task<AuthorDTO?> Add(AuthorDTO authorDTO);
        public Task<AuthorDTO?> Update(AuthorDTO authorDTO);
        public Task<bool> Remove(string id);
    }
}