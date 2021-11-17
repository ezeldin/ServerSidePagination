using FinancialExecution.Reconcil.Domain.Constants;
using FinancialExecution.Reconcil.Domain.Helpers;
using FinancialExecution.Reconcil.GenericTable;
using FinancialExecution.Reconcil.Web.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FinancialExecution.Reconcil.Web.Controllers
{
    public class BaseController : Controller
    {
        public BaseController()
        {
        }

        [AllowAnonymous]
        public async Task<IRestResponse> ExecuteApi(string url, Method method, DataFormat format, object body,
            string header = "application/json")
        {
            var client = new RestClient(AppsettingsHelper.GetValue(Keys.ReconcilApi) + $"{url}");
            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;
            var request = new RestRequest(method) { RequestFormat = format };
            var accessToken = HttpContext.Request.Cookies["JWToken"];
            if (accessToken != null && accessToken.Length > 0)
            {
                request.AddParameter("Authorization", "Bearer " + accessToken, ParameterType.HttpHeader);
            }
            if (body != null)
            {
                if (method == Method.GET)
                    request.AddParameter("param", body);
                else request.AddJsonBody(body);
            }
            request.AddHeader("Content-Type", header);
            request.AddHeader("ClientIP", Convert.ToString(HttpContext.Connection.RemoteIpAddress));  //request Ip address
            request.AddHeader("Reference", Guid.NewGuid().ToString());  //  request reference
            //request.AddHeader("Browser", HttpContext.Request.Headers["User-Agent"].ToString());  //request browser
            var result = await Task.FromResult(client.Execute(request));
            return result;
        }
       
    }
}