using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetPublisherResponse : BaseResponse
    {
        public PublisherDTO Publisher { get; set; }
    }
}