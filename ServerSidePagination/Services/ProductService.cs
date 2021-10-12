using Dapper;
using Helpers;
using Models;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Services
{
    public class ProductService
    {
        public ProductViewModel GetAll(ProductParams param)
        {
            var query = new StringBuilder();
            query.AppendQuery("GetAllRequests.sql");
            using (var connection = new SqlConnection(Utility.GetConnectionString()))
            {
                connection.Open();
                var result = connection.Query<ProductDto>(query.ToString()
                    , new
                    {
                        @PageNo = param.iDisplayStart,
                        @PageSize = param.iDisplayLength,
                        @ProductName = param.ProductName,
                    }, null, true, 60 * 3);

                ProductViewModel model = new ProductViewModel();
                model.iTotalRecords = result.Count() > 0 ? result.FirstOrDefault().TotalCount : 0;
                model.Products = result.ToList();
                return model;
            }
        }
    }


}