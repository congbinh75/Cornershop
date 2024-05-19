using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cornershop.Service.Infrastructure.Entities
{
    public class BaseEntity
    {
        [Key]
        //Not working with SQL Server as string data type
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public bool IsDeleted { get; set; } = false;

        public DateTimeOffset CreatedOn { get; set; }

        [NotMapped]
        public User? CreatedBy { get; set; }

        public string? CreatedById { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }

        [NotMapped]
        public User? UpdatedBy { get; set; }

        public string? UpdatedById { get; set; }
    }
}
