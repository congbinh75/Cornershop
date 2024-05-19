using Cornershop.Service.Common;
using Cornershop.Service.Domain.Interfaces;
using Cornershop.Service.Infrastructure.Contexts;
using Cornershop.Shared.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Domain.Services
{
    public class SubcategoryService : ISubCategoryService
    {
        public Task<SubcategoryDTO?> Add(SubcategoryDTO categoryDTO)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<SubcategoryDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<SubcategoryDTO?> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(string id)
        {
            throw new NotImplementedException();
        }

        public Task<SubcategoryDTO?> Update(SubcategoryDTO categoryDTO)
        {
            throw new NotImplementedException();
        }
    }
}