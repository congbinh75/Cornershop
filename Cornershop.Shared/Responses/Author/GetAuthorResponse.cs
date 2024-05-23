using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetAuthorResponse : BaseResponse
    {
        public AuthorDTO Author { get; set; }
    }
}