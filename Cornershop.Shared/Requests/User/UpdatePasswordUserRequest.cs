namespace Cornershop.Shared.Requests
{
    public class UpdatePasswordRequest
    {
        public string Id { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
