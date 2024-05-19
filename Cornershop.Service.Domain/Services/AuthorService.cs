using Cornershop.Service.Common;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services
{
    public class AuthorService : IAuthorService
    {
        public Task<AuthorDTO?> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<AuthorDTO?> Add(AuthorDTO authorDTO)
        {
            throw new NotImplementedException();
        }

        public Task<AuthorDTO?> Update(AuthorDTO authorDTO)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(string id)
        {
            throw new NotImplementedException();
        }
    }
}