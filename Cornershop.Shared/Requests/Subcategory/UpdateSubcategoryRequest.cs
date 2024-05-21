namespace Cornershop.Shared.Requests
{
    public class UpdateSubcategoryRequest
    {
        public string Id { get; set; }
        
        public string Name { get; set; }

        public string CategoryId { get; set; }

        public string Description { get; set; }
    }
}
