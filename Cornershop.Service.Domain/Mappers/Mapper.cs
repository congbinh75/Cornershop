using Cornershop.Service.Common.DTOs;
using Cornershop.Service.Infrastructure.Entities;

namespace Cornershop.Service.Domain
{
    public static class Mapper 
    {
        public static UserDTO? Map(User? user)
        {
            if (user == null) return null;
            return new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsBanned = user.IsBanned,
                Role = user.Role,
                CreatedOn = user.CreatedOn,
                UpdatedOn = user.UpdatedOn,
            };
        }

        public static User? Map(UserDTO? userDTO)
        {
            if (userDTO == null) return null;
            return new User
            {
                Id = userDTO.Id!,
                Username = userDTO.Username!,
                FirstName = userDTO.FirstName!,
                LastName = userDTO.LastName!,
                Email = userDTO.Email!,
                IsBanned = userDTO.IsBanned ?? false,
                Role = userDTO.Role ?? 0,
                IsEmailConfirmed = userDTO.IsEmailConfirmed ?? false,
                Password = "",
                Salt = [],
                CreatedOn = userDTO.CreatedOn,
                UpdatedOn = userDTO.UpdatedOn
            };
        }

        public static CategoryDTO? Map(Category? category)
        {
            if (category == null) return null;
            return new CategoryDTO
            {
                Name = category.Name,
                Description = category.Description,
                CreatedOn = category.CreatedOn,
                CreatedBy = Map(category.CreatedBy),
                UpdatedOn = category.UpdatedOn,
                UpdatedBy = Map(category.UpdatedBy)
            };
        }

        public static Category? Map(CategoryDTO? categoryDTO)
        {
            if (categoryDTO == null) return null;
            return new Category
            {
                Name = categoryDTO.Name!,
                Description = categoryDTO.Description!,
                CreatedOn = categoryDTO.CreatedOn,
                CreatedBy = Map(categoryDTO.CreatedBy),
                UpdatedOn = categoryDTO.UpdatedOn,
                UpdatedBy = Map(categoryDTO.UpdatedBy)
            };
        }

        public static ProductDTO? Map(Product? product)
        {
            if (product == null) return null;
            var ratingVoteDTOs = product.RatingVotes.Select(v => Mapper.Map(v)).ToList();
            return new ProductDTO
            {
                Name = product.Name,
                Code = product.Code,
                Description = product.Description,
                Category = Map(product.Category),
                Price = product.Price,
                ImagesUrls = product.ImagesUrls,
                Rating = product.Rating,
                RatingVotes = product.RatingVotes.Select(Map).ToList()!,
                CreatedOn = product.CreatedOn,
                CreatedBy = Map(product.CreatedBy),
                UpdatedOn = product.UpdatedOn,
                UpdatedBy = Map(product.UpdatedBy)
            };
        }

        public static Product? Map(ProductDTO? productDTO)
        {
            if (productDTO == null) return null;
            var ratingVoteDTOs = productDTO.RatingVotes.Select(Map).ToList();
            return new Product
            {
                Name = productDTO.Name ?? string.Empty,
                Code = productDTO.Code ?? string.Empty,
                Description = productDTO.Description ?? string.Empty,
                Category = Map(productDTO.Category),
                Price = productDTO.Price ?? 0,
                ImagesUrls = productDTO.ImagesUrls,
                Rating = productDTO.Rating ?? 0,
                RatingVotes = productDTO.RatingVotes.Select(Map).ToList(),
                CreatedOn = productDTO.CreatedOn,
                CreatedBy = Map(productDTO.CreatedBy),
                UpdatedOn = productDTO.UpdatedOn,
                UpdatedBy = Map(productDTO.UpdatedBy)
            };
        }

        public static RatingVoteDTO? Map(RatingVote? ratingVote)
        {
            if (ratingVote == null) return null;
            return new RatingVoteDTO
            {
                Product = Map(ratingVote.Product),
                User = Map(ratingVote.User),
                Rate = ratingVote.Rate
            };
        }

        public static RatingVote? Map(RatingVoteDTO? ratingVoteDTO)
        {
            if (ratingVoteDTO == null) return null;
            return new RatingVote
            {
                Product = Map(ratingVoteDTO.Product)!,
                ProductId = ratingVoteDTO.Product!.Id!,
                User = Map(ratingVoteDTO.User)!,
                UserId = ratingVoteDTO.User!.Id!,
                Rate = ratingVoteDTO.Rate
            };
        }
    }
}