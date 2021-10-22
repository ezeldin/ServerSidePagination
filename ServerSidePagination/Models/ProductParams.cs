namespace Models
{
    public class ProductParams : DataTableParams
    {
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
    }

    public class ProductExportParams
    {
        public string ProductName { get; set; }
        public decimal? Price { get; set; }
    }
}