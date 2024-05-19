namespace Cornershop.Shared.DTOs
{
    public class TransactionDTO : BaseDTO
    {
        public OrderDTO Order { get; set; }

        public decimal Amount { get; set; }
    }
}