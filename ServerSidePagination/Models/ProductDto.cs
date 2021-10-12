namespace Models
{
    public class ProductDto
    {
        public int TotalCount { get; set; }
        public int Id { get; set; }
        public string ProductName { get; set; }
        public short ProductYear { get; set; }
        public decimal Price { get; set; }
    }
}