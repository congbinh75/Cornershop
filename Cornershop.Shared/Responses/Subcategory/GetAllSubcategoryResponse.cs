using System.Collections.Generic;
using Cornershop.Shared.DTOs;

namespace Cornershop.Shared.Responses
{
    public class GetAllSubcategoryResponse : BaseResponse
    {
        public ICollection<SubcategoryDTO> Subcategories { get; set; }
        public int PagesCount { get; set; }
    }
}