namespace Cornershop.Shared.Responses
{
    public class BaseResponse
    {
        public string Status { get; set; } = "Success";
        public string Message { get; set; } = string.Empty;
    }
}