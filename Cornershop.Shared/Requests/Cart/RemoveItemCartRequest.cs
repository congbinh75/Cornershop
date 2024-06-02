namespace Cornershop.Shared.Requests
{
    public class RemoveItemCartRequest
    {
        public string ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
