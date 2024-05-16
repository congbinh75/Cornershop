using System.Collections.Generic;
using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetAllCategoryResponse : BaseResponse
    {
        public ICollection<CategoryDTO> CategoryList { get; set; }
    }
}