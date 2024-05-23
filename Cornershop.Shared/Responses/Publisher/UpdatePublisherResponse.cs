using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class UpdatePublisherResponse : BaseResponse
    {
        public PublisherDTO Publisher { get; set; }
    }
}