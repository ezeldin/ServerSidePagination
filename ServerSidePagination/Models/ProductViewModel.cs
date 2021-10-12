using System.Collections.Generic;

namespace Models
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            Products = new List<ProductDto>();
        }
        public int iTotalRecords { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}