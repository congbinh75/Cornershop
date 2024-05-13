namespace Cornershop.Service.Common.DTOs
{
    public class UserDTO
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public string? PlainPassword { get; set; }

        public bool? IsBanned { get; set; }

        public int? Role { get; set; }
    }
}
