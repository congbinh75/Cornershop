using Cornershop.Shared.DTOs;

namespace Cornershop.Service.Domain.Interfaces
{
    public interface IPublisherService
    {
        public Task<PublisherDTO?> GetById(string id);
        public Task<PublisherDTO?> Add(PublisherDTO authorDTO);
        public Task<PublisherDTO?> Update(PublisherDTO authorDTO);
        public Task<bool> Remove(string id);
    }
}