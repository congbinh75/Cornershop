using System.Collections.Generic;
using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetAllAuthorResponse : BaseResponse
    {
        public ICollection<AuthorDTO> Authors { get; set; }

        public int PagesCount { get; set; }
    }
}