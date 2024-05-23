using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class AddAuthorResponse : BaseResponse
    {
        public AuthorDTO Author { get; set; }
    }
}