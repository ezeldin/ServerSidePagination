using ClosedXML.Excel;
using Models;
using Rotativa;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ServerSidePagination.Controllers
{
    public class HomeController : Controller
    {
        private readonly ProductService _productService;
        public HomeController()
        {
            _productService = new ProductService();
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetAllProducts(ProductParams inputParams)
        {
            var results = _productService.GetAll(inputParams);

            var jsonResult = Json(new
            {
                sEcho = inputParams.sEcho,
                iTotalRecords = results.iTotalRecords,
                iTotalDisplayRecords = results.iTotalRecords,
                aaData = results.Products
            },
                JsonRequestBehavior.AllowGet);

            return jsonResult;
        }

        [HttpPost]
        public ActionResult ExportProductsToCSV(ProductExportParams inputParams)
        {
            var products = _productService.ExportToExcel(inputParams);

            var builder = new StringBuilder();
            builder.AppendLine("Id,ProductName,ProductYear,Price");
            products.ForEach(x =>
            {
                builder.AppendLine($"{x.Id},{x.ProductName},{x.ProductYear},{x.Price}");
            });

            return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "products.csv");
        }

        public ActionResult ExportProductsToExcel(ProductExportParams inputParams)
        {
            var products = _productService.ExportToExcel(inputParams);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("products");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "ProductName";
                worksheet.Cell(currentRow, 3).Value = "ProductYear";
                worksheet.Cell(currentRow, 4).Value = "Price";

                foreach (var product in products)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = product.Id;
                    worksheet.Cell(currentRow, 2).Value = product.ProductName;
                    worksheet.Cell(currentRow, 3).Value = product.ProductYear;
                    worksheet.Cell(currentRow, 4).Value = product.Price;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "products.xlsx");
                }
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> PrintProduct(long id)
        {
            var product =await _productService.ExportToPdf(id);
            return new ViewAsPdf(product);
        }
    }
}