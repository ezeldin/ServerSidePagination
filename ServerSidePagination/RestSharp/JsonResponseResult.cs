//using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace FinancialExecution.Reconcil.Web.Utils
{
    //public class JsonResponseResult : JsonResult
    //{
    //    private readonly HttpStatusCode _statusCode;

    //    public JsonResponseResult(HttpStatusCode statusCode, object value)
    //        : base(value)
    //    {
    //        _statusCode = statusCode;
    //    }

    //    public override async Task ExecuteResultAsync(ActionContext context)
    //    {
    //        context.HttpContext.Response.StatusCode = (int)_statusCode;
    //        await base.ExecuteResultAsync(context);
    //    }

    //    public override void ExecuteResult(ActionContext context)
    //    {
    //        context.HttpContext.Response.StatusCode = (int)_statusCode;

    //        base.ExecuteResult(context);
    //    }
    //}

    public static class FormatApiContent
    {
        public static object ToJson<T>(this string content, HttpStatusCode httpStatusCode)
        {
            switch (httpStatusCode)
            {
                case HttpStatusCode.MethodNotAllowed:
                case HttpStatusCode.InternalServerError:
                    return JsonConvert.DeserializeObject<Dictionary<string, string>>(content)["Message"];
                case HttpStatusCode.BadRequest:
                    try
                    {
                        return JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(content);
                    }
                    catch
                    {
                        return content;
                    }
                case HttpStatusCode.OK:
                    try
                    {
                        var result = JsonConvert.DeserializeObject<T>(content);
                        return result;
                    }
                    catch
                    {
                        return content;
                    }
                default:
                    return content;
            }
        }
    }
}
