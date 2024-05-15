using Cornershop.Shared.DTOs;
using Cornershop.Service.Infrastructure.Entities;

namespace Cornershop.Service.Domain
{
    public static class Mapper 
    {
        public static UserDTO Map(User user)
        {
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

        public static User Map(UserDTO userDTO)
        {
            return new User
            {
                Id = userDTO.Id,
                Username = userDTO.Username,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                IsBanned = userDTO.IsBanned,
                Role = userDTO.Role,
                IsEmailConfirmed = userDTO.IsEmailConfirmed,
                Password = "",
                Salt = [],
                CreatedOn = userDTO.CreatedOn,
                UpdatedOn = userDTO.UpdatedOn
            };
        }

        public static CategoryDTO Map(Category category)
        {
            return new CategoryDTO
            {
                Name = category.Name,
                Description = category.Description,
                CreatedOn = category.CreatedOn,
                CreatedBy = category.CreatedBy == null ? null : Map(category.CreatedBy),
                UpdatedOn = category.UpdatedOn,
                UpdatedBy = category.UpdatedBy == null ? null : Map(category.UpdatedBy)
            };
        }

        public static Category Map(CategoryDTO categoryDTO)
        {
            return new Category
            {
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                CreatedOn = categoryDTO.CreatedOn,
                CreatedBy = Map(categoryDTO.CreatedBy),
                UpdatedOn = categoryDTO.UpdatedOn,
                UpdatedBy = Map(categoryDTO.UpdatedBy)
            };
        }

        public static ProductDTO Map(Product product)
        {
            var ratingVoteDTOs = product.RatingVotes.Select(Map).ToList();
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
                CreatedBy = product.CreatedBy == null ? null : Map(product.CreatedBy),
                UpdatedOn = product.UpdatedOn,
                UpdatedBy = product.UpdatedBy == null ? null : Map(product.UpdatedBy)
            };
        }

        public static Product Map(ProductDTO productDTO)
        {
            var ratingVoteDTOs = productDTO.RatingVotes.Select(Map).ToList();
            return new Product
            {
                Name = productDTO.Name,
                Code = productDTO.Code,
                Description = productDTO.Description,
                Category = Map(productDTO.Category),
                Price = productDTO.Price,
                ImagesUrls = productDTO.ImagesUrls,
                Rating = productDTO.Rating,
                RatingVotes = productDTO.RatingVotes.Select(Map).ToList(),
                CreatedOn = productDTO.CreatedOn,
                CreatedBy = Map(productDTO.CreatedBy),
                UpdatedOn = productDTO.UpdatedOn,
                UpdatedBy = Map(productDTO.UpdatedBy)
            };
        }

        public static RatingVoteDTO Map(RatingVote ratingVote)
        {
            return new RatingVoteDTO
            {
                Product = Map(ratingVote.Product),
                User = Map(ratingVote.User),
                Rate = ratingVote.Rate
            };
        }

        public static RatingVote Map(RatingVoteDTO ratingVoteDTO)
        {
            return new RatingVote
            {
                Product = Map(ratingVoteDTO.Product),
                ProductId = ratingVoteDTO.Product.Id,
                User = Map(ratingVoteDTO.User),
                UserId = ratingVoteDTO.User.Id,
                Rate = ratingVoteDTO.Rate
            };
        }
    }
}