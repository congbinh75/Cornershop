using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Cornershop.Presentation.Customer.Interfaces;
using Cornershop.Shared.DTOs;
using Cornershop.Shared.Requests;
using Cornershop.Shared.Responses;

namespace Cornershop.Presentation.Customer.Services;

public class ReviewService(IHttpClientFactory httpClientFactory, IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IReviewService
{
    public async Task<(ICollection<ReviewDTO> reviews, int count)> GetAllByProduct(string productId, int page, int pageSize)
    {
        var httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
        var httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + "api/review" + "?page=" + page + "&pageSize=" + pageSize + "&productId=" + productId);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            var data = await httpResponseMessage.Content.ReadAsStringAsync();
            var response = JsonSerializer.Deserialize<GetAllReviewByProductResponse>(data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (response != null && response.Reviews != null)
            {
                return (response.Reviews, response.PagesCount);
            }
        }
        return ([], 0);
    }

    public async Task<ReviewDTO?> GetReviewOfProductByCurrentUser(string productId)
    {
        var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthCookie"];
        if (token != null)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpResponseMessage = await httpClient.GetAsync(httpClient.BaseAddress + "api/review/current-user" + "?productId=" + productId);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var data = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<GetReviewOfProductByCurrentUserResponse>(data, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (response != null && response.Review != null)
                {
                    return response.Review;
                }
            }
        }
        return null;
    }

    public async Task<ReviewDTO?> Add(string productId, int rating, string comment)
    {
        var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthCookie"];
        if (token != null)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var reviewRequest = new AddReviewRequest()
            {
                ProductId = productId,
                Rating = rating,
                Comment = comment
            };
            var jsonContent = new StringContent(JsonSerializer.Serialize(reviewRequest), Encoding.UTF8, "application/json");

            var httpResponseMessage = await httpClient.PutAsync(httpClient.BaseAddress + "api/review", jsonContent);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var data = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<AddReviewResponse>(data, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (response != null && response.Review != null)
                {
                    return response.Review;
                }
            }
        }
        return null;
    }

    public async Task<bool> Remove(string productId)
    {
        var token = httpContextAccessor.HttpContext?.Request.Cookies["AuthCookie"];
        if (token != null)
        {
            var httpClient = httpClientFactory.CreateClient();
            httpClient.BaseAddress = new Uri(configuration["Service:BaseAddress"] ?? "");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var httpResponseMessage = await httpClient.DeleteAsync(httpClient.BaseAddress + "api/review" + "?id" + productId);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var data = await httpResponseMessage.Content.ReadAsStringAsync();
                var response = JsonSerializer.Deserialize<RemoveReviewResponse>(data, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                if (response != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
}