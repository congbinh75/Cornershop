using System.Collections.Generic;
using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetAllPublisherResponse : BaseResponse
    {
        public ICollection<PublisherDTO> Publishers { get; set; }

        public int PagesCount { get; set; }
    }
}