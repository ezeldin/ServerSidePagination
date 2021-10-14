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
        
        BasicHttpBinding basicHttpBinding = new BasicHttpBinding();
EndpointAddress endpointAddress = 
                new EndpointAddress("https://localhost:8082/OrderManagement/OrderManagement.svc");
OrderManagementClient wcfClient = 
                new OrderManagementClient(basicHttpBinding, endpointAddress);

//wcfClient.ClientCredentials.ServiceCertificate.SslCertificateAuthentication
//is need to be set for Https certificate authentication
wcfClient.ClientCredentials.ServiceCertificate.SslCertificateAuthentication =
	new X509ServiceCertificateAuthentication()
	{
		CertificateValidationMode = X509CertificateValidationMode.None,
		RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck
	};

var result = wcfClient.GetOrderByID(1);
    }
}
