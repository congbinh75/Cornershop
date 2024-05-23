using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class UpdateAuthorResponse : BaseResponse
    {
        public AuthorDTO Author { get; set; }
    }
}