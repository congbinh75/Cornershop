using System;

namespace Cornershop.Shared.DTOs
{
    public class BaseDTO
    {
        public string Id { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public UserDTO CreatedBy { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }

        public UserDTO UpdatedBy { get; set; }
    }
}
