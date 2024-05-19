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
                CreatedBy = user.CreatedBy != null ? Map(user.CreatedBy) : null,
                UpdatedOn = user.UpdatedOn,
                UpdatedBy = user.UpdatedBy != null ? Map(user.UpdatedBy) : null,
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
                Reviews = userDTO.Reviews.Select(Map).ToList(),
                Cart = Map(userDTO.Cart),
                Orders = userDTO.Orders.Select(Map).ToList(),
                CreatedOn = userDTO.CreatedOn,
                CreatedBy = Map(userDTO.CreatedBy),
                UpdatedOn = userDTO.UpdatedOn,
                UpdatedBy = Map(userDTO.UpdatedBy)
            };
        }

        public static CategoryDTO Map(Category category)
        {
            return new CategoryDTO
            {
                Id = category.Id,
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
                Id = categoryDTO.Id,
                Name = categoryDTO.Name,
                Description = categoryDTO.Description,
                CreatedOn = categoryDTO.CreatedOn,
                CreatedBy = Map(categoryDTO.CreatedBy),
                UpdatedOn = categoryDTO.UpdatedOn,
                UpdatedBy = Map(categoryDTO.UpdatedBy)
            };
        }

        public static SubcategoryDTO Map(Subcategory subcategory)
        {
            return new SubcategoryDTO
            {
                Id = subcategory.Id,
                Name = subcategory.Name,
                Description = subcategory.Description,
                Category = Map(subcategory.Category),
                CreatedOn = subcategory.CreatedOn,
                CreatedBy = subcategory.CreatedBy == null ? null : Map(subcategory.CreatedBy),
                UpdatedOn = subcategory.UpdatedOn,
                UpdatedBy = subcategory.UpdatedBy == null ? null : Map(subcategory.UpdatedBy)
            };
        }

        public static Subcategory Map(SubcategoryDTO subcategoryDTO)
        {
            return new Subcategory
            {
                Id = subcategoryDTO.Id,
                Name = subcategoryDTO.Name,
                Description = subcategoryDTO.Description,
                Category = Map(subcategoryDTO.Category),
                CreatedOn = subcategoryDTO.CreatedOn,
                CreatedBy = Map(subcategoryDTO.CreatedBy),
                UpdatedOn = subcategoryDTO.UpdatedOn,
                UpdatedBy = Map(subcategoryDTO.UpdatedBy)
            };
        }

        public static ProductDTO Map(Product product)
        {
            var ratingVoteDTOs = product.Reviews.Select(Map).ToList();
            return new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Code = product.Code,
                Description = product.Description,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                Width = product.Width,
                Length = product.Length,
                Height = product.Height,
                Pages = product.Pages,
                Format = product.Format,
                Stock = product.Stock,
                PublishedYear = product.PublishedYear,
                ImagesUrls = product.ImagesUrls,
                Rating = product.Rating,
                Reviews = product.Reviews.Select(Map).ToList()!,
                CreatedOn = product.CreatedOn,
                CreatedBy = product.CreatedBy == null ? null : Map(product.CreatedBy),
                UpdatedOn = product.UpdatedOn,
                UpdatedBy = product.UpdatedBy == null ? null : Map(product.UpdatedBy)
            };
        }

        public static Product Map(ProductDTO productDTO)
        {
            var ratingVoteDTOs = productDTO.Reviews.Select(Map).ToList();
            return new Product
            {
                Id = productDTO.Id,
                Name = productDTO.Name,
                Code = productDTO.Code,
                Description = productDTO.Description,
                Subcategory = Map(productDTO.Subcategory),
                SubcategoryId = productDTO.Subcategory.Id,
                Price = productDTO.Price,
                OriginalPrice = productDTO.OriginalPrice,
                Width = productDTO.Width,
                Length = productDTO.Length,
                Height = productDTO.Height,
                Pages = productDTO.Pages,
                Format = productDTO.Format,
                Stock = productDTO.Stock,
                PublishedYear = productDTO.PublishedYear,
                ImagesUrls = productDTO.ImagesUrls,
                Rating = productDTO.Rating,
                Reviews = productDTO.Reviews.Select(Map).ToList(),
                Authors = productDTO.Authors.Select(Map).ToList(),
                Publisher = Map(productDTO.Publisher),
                IsVisible = productDTO.IsVisible,
                CreatedOn = productDTO.CreatedOn,
                CreatedBy = Map(productDTO.CreatedBy),
                UpdatedOn = productDTO.UpdatedOn,
                UpdatedBy = Map(productDTO.UpdatedBy)
            };
        }

        public static ReviewDTO Map(Review review)
        {
            return new ReviewDTO
            {
                Id = review.Id,
                Product = Map(review.Product),
                User = Map(review.User),
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedOn = review.CreatedOn,
                CreatedBy = review.CreatedBy != null ? Map(review.CreatedBy) : null,
                UpdatedOn = review.UpdatedOn,
                UpdatedBy = review.UpdatedBy != null ? Map(review.UpdatedBy) : null
            };
        }

        public static Review Map(ReviewDTO reviewDTO)
        {
            return new Review
            {
                Id = reviewDTO.Id,
                Product = Map(reviewDTO.Product),
                User = Map(reviewDTO.User),
                Rating = reviewDTO.Rating,
                Comment = reviewDTO.Comment,
                CreatedOn = reviewDTO.CreatedOn,
                CreatedBy = Map(reviewDTO.CreatedBy),
                UpdatedOn = reviewDTO.UpdatedOn,
                UpdatedBy = Map(reviewDTO.UpdatedBy)
            };
        }

        public static AuthorDTO Map(Author author)
        {
            return new AuthorDTO
            {
                Id = author.Id,
                Name = author.Name,
                Description = author.Description,
                Products = author.Products.Select(Map).ToList(),
                CreatedOn = author.CreatedOn,
                CreatedBy = author.CreatedBy != null ? Map(author.CreatedBy) : null,
                UpdatedOn = author.UpdatedOn,
                UpdatedBy = author.UpdatedBy != null ? Map(author.UpdatedBy) : null
            };
        }

        public static Author Map(AuthorDTO authorDTO)
        {
            return new Author
            {
                Id = authorDTO.Id,
                Name = authorDTO.Name,
                Description = authorDTO.Description,
                Products = authorDTO.Products.Select(Map).ToList(),
                CreatedBy = Map(authorDTO.CreatedBy),
                CreatedOn = authorDTO.CreatedOn,
                UpdatedBy = Map(authorDTO.UpdatedBy),
                UpdatedOn = authorDTO.UpdatedOn
            };
        }

        public static PublisherDTO Map(Publisher publisher)
        {
            return new PublisherDTO
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Description = publisher.Description,
                Products = publisher.Products.Select(Map).ToList(),
                CreatedOn = publisher.CreatedOn,
                CreatedBy = publisher.CreatedBy != null ? Map(publisher.CreatedBy) : null,
                UpdatedOn = publisher.UpdatedOn,
                UpdatedBy = publisher.UpdatedBy != null ? Map(publisher.UpdatedBy) : null
            };
        }

        public static Publisher Map(PublisherDTO publisherDTO)
        {
            return new Publisher
            {
                Id = publisherDTO.Id,
                Name = publisherDTO.Name,
                Description = publisherDTO.Description,
                Products = publisherDTO.Products.Select(Map).ToList(),
                CreatedBy = Map(publisherDTO.CreatedBy),
                CreatedOn = publisherDTO.CreatedOn,
                UpdatedBy = Map(publisherDTO.UpdatedBy),
                UpdatedOn = publisherDTO.UpdatedOn,
            };
        }

        public static OrderDTO Map(Order order)
        {
            return new OrderDTO
            {
                Id = order.Id,
                User = Map(order.User),
                Code = order.Code,
                OrderDetails = order.OrderDetails.Select(Map).ToList(),
                TotalPrice = order.TotalPrice,
                Transactions = order.Transactions.Select(Map).ToList(),
                CreatedBy = order.CreatedBy != null ? Map(order.CreatedBy) : null,
                CreatedOn = order.CreatedOn,
                UpdatedBy = order.UpdatedBy != null ? Map(order.UpdatedBy) : null,
                UpdatedOn = order.UpdatedOn,
            };
        }

        public static Order Map(OrderDTO orderDTO)
        {
            return new Order
            {
                Id = orderDTO.Id,
                User = Map(orderDTO.User),
                Code = orderDTO.Code,
                OrderDetails = orderDTO.OrderDetails.Select(Map).ToList(),
                TotalPrice = orderDTO.TotalPrice,
                Transactions = orderDTO.Transactions.Select(Map).ToList(),
                CreatedBy = Map(orderDTO.CreatedBy),
                CreatedOn = orderDTO.CreatedOn,
                UpdatedBy = Map(orderDTO.UpdatedBy),
                UpdatedOn = orderDTO.UpdatedOn,
            };
        }

        public static OrderDetailDTO Map(OrderDetail orderDetail)
        {
            return new OrderDetailDTO
            {
                Order = Map(orderDetail.Order),
                Product = Map(orderDetail.Product),
                Quantity = orderDetail.Quantity,
                Price = orderDetail.Price
            };
        }

        public static OrderDetail Map(OrderDetailDTO orderDetailDTO)
        {
            return new OrderDetail
            {
                Order = Map(orderDetailDTO.Order),
                OrderId = orderDetailDTO.Order.Id,
                Product = Map(orderDetailDTO.Product),
                ProductId = orderDetailDTO.Product.Id,
                Quantity = orderDetailDTO.Quantity,
                Price = orderDetailDTO.Price
            };
        }
        
        public static TransactionDTO Map(Transaction transaction)
        {
            return new TransactionDTO
            {
                Id = transaction.Id,
                Order = Map(transaction.Order),
                Amount = transaction.Amount,
                CreatedOn = transaction.CreatedOn,
                CreatedBy = transaction.CreatedBy != null ? Map(transaction.CreatedBy) : null,
                UpdatedOn = transaction.UpdatedOn,
                UpdatedBy = transaction.UpdatedBy != null ? Map(transaction.UpdatedBy) : null
            };
        }

        public static Transaction Map(TransactionDTO transactionDTO)
        {
            return new Transaction
            {
                Id = transactionDTO.Id,
                Order = Map(transactionDTO.Order),
                Amount = transactionDTO.Amount,
                CreatedBy = Map(transactionDTO.CreatedBy),
                CreatedOn = transactionDTO.CreatedOn,
                UpdatedBy = Map(transactionDTO.UpdatedBy),
                UpdatedOn = transactionDTO.UpdatedOn,
            };
        }

        public static CartDTO Map(Cart cart)
        {
            return new CartDTO
            {
                User = Map(cart.User),
                CartDetails = cart.CartDetails.Select(Map).ToList()
            };
        }

        public static Cart Map(CartDTO cartDTO)
        {
            return new Cart
            {
                User = Map(cartDTO.User),
                UserId = cartDTO.User.Id,
                CartDetails = cartDTO.CartDetails.Select(Map).ToList()
            };
        }

        public static CartDetailDTO Map(CartDetail cartDetail)
        {
            return new CartDetailDTO
            {
                Cart = Map(cartDetail.Cart),
                Product = Map(cartDetail.Product),
                Quantity = cartDetail.Quantity,
                AddedOn = cartDetail.AddedOn
            };
        }

        public static CartDetail Map(CartDetailDTO cartDetailDTO)
        {
            return new CartDetail
            {
                Cart = Map(cartDetailDTO.Cart),
                CartId = cartDetailDTO.Cart.Id,
                Product = Map(cartDetailDTO.Product),
                ProductId = cartDetailDTO.Product.Id,
                Quantity = cartDetailDTO.Quantity,
                AddedOn = cartDetailDTO.AddedOn
            };
        }
    }
}