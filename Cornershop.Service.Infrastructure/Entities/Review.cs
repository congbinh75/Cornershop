using Microsoft.EntityFrameworkCore;

namespace Cornershop.Service.Infrastructure.Entities
{
    [Index(nameof(Id))]
    public class Review : BaseEntity
    {
        public required Product Product { get; set; }

        public required User User { get; set; }

        public required int Rating { get; set; }

        public required string Comment { get; set; }
    }
}