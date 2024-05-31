using Cornershop.Service.Common;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IAuthorService
    {
        public Task<Result<AuthorDTO?, string?>> GetById(string id);
        public Task<(ICollection<AuthorDTO> authors, int count)> GetAll(int page, int pageSize);
        public Task<Result<AuthorDTO?, string?>> Add(AuthorDTO authorDTO);
        public Task<Result<AuthorDTO?, string?>> Update(AuthorDTO authorDTO);
        public Task<Result<bool, string?>> Remove(string id);
    }
}