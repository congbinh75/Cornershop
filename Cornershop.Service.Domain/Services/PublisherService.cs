using Cornershop.Service.Common;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services
{
    public class PublisherService : IPublisherService
    {
        public Task<PublisherDTO?> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PublisherDTO?> Add(PublisherDTO authorDTO)
        {
            throw new NotImplementedException();
        }

        public Task<PublisherDTO?> Update(PublisherDTO authorDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(string id)
        {
            throw new NotImplementedException();
        }
    }
}