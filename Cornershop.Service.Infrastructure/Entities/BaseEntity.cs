using System.ComponentModel.DataAnnotations;

namespace Cornershop.Service.Infrastructure.Entities
{
    public class BaseEntity
    {
        [Key]
        //Not working with SQL Server as string data type
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public bool IsDeleted { get; set; } = false;

        public DateTimeOffset CreatedTime { get; set; }

        public User? CreatedBy { get; set; }

        public DateTimeOffset UpdatedTime { get; set; }

        public User? UpdatedBy { get; set; }
    }
}
