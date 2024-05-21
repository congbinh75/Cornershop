using System.Collections.Generic;
using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetAllCategoryResponse : BaseResponse
    {
        public ICollection<CategoryDTO> Categories { get; set; }

        public int PagesCount { get; set; }
    }
}