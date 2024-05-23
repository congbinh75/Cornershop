using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class AddPublisherResponse : BaseResponse
    {
        public PublisherDTO Publisher { get; set; }
    }
}