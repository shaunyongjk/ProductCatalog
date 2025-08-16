namespace ProductCatalogAPI.Models
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public decimal Price { get; set; } // 20% markup
    }
    public class ApiSettings
    {
        public string VendorBaseUrl { get; set; }
        public string Key { get; set; }
    }
}
