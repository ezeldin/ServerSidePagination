using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
