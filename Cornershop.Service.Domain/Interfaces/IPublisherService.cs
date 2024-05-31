using Cornershop.Service.Common;
using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IPublisherService
    {
        public Task<Result<PublisherDTO?, string?>> GetById(string id);
        public Task<(ICollection<PublisherDTO> publishers, int count)> GetAll(int page, int pageSize);
        public Task<Result<PublisherDTO?, string?>> Add(PublisherDTO authorDTO);
        public Task<Result<PublisherDTO?, string?>> Update(PublisherDTO authorDTO);
        public Task<Result<bool, string?>> Remove(string id);
    }
}