//using FinancialExecution.Reconcil.Domain.Common;
//using FinancialExecution.Reconcil.Domain.Dto;
//using FinancialExecution.Reconcil.Domain.Dto.ReconciliationOrder.Details;
//using FinancialExecution.Reconcil.GenericTable;
//using FinancialExecution.Reconcil.Web.Utils;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Newtonsoft.Json;
//using RestSharp;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Threading.Tasks;

//namespace FinancialExecution.Reconcil.Web.Controllers
//{
//    public class ReconciliationController : BaseController
//    {

//        public IActionResult Index()
//        {
//            AccountsListDDL();
//            return View();
//        }

//        #region RO
//        public IActionResult Create()
//        {
//            AccountsListDDL();
//            return View();
//        }

//        [HttpPost]
//        public async Task<ActionResult> SaveReconciliationOrder(CreateReconcilOrderInput data)
//        {
//            var result = await ExecuteApi("ReconcilOrders", Method.POST, DataFormat.Json, data);
//            return new JsonResponseResult(result.StatusCode, result.Content);
//        }

//        [HttpPost]
//        public async Task<ActionResult> GetReconciliationOrders([FromBody] DTParameters<ReconciliationOrderSearchDto> param)
//        {
//            var result = ExecuteApi("ReconcilOrders/SearchReconciliationOrder/", Method.POST, DataFormat.Json, param);
//            var model = JsonConvert.DeserializeObject<TableResult<ReconciliationOrderOutputDto>>(result.Result.Content, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
//            if (result.Result.StatusCode == HttpStatusCode.OK)
//            {
//                var resp = new DTResult<ReconciliationOrderOutputDto>();
//                resp.data = model.FilteredList;
//                resp.draw = param.Draw;
//                resp.recordsFiltered = (int)model.TotalCount;
//                resp.recordsTotal = (int)model.TotalCount;
//                return Json(resp);
//            }
//            return new JsonResponseResult(result.Result.StatusCode, result.Result.Content);
//        }
//        #endregion

//        #region ReconcilOrder Actions

//        [HttpPost]
//        public async Task<ActionResult> PrepareReconcilOrder(PrepareReconcilOrderInput input)
//        {
//            var result = await ExecuteApi("ReconcilOrders/PrepareReconcilOrder/", Method.POST, DataFormat.Json, input);
//            return new JsonResponseResult(result.StatusCode, result.Content);
//        }

//        [HttpPost]
//        public async Task<ActionResult> RunReconcilOrder(PrepareReconcilOrderInput input)
//        {
//            var result = await ExecuteApi("ReconcilOrders/RunReconcilOrder/", Method.POST, DataFormat.Json, input);
//            return new JsonResponseResult(result.StatusCode, result.Content);
//        }

//        #endregion

//        #region RO Details

//        [HttpGet]
//        public async Task<ActionResult> Details(long id)
//        {
//            var result = await ExecuteApi("ReconcilOrders/Details/" + id, Method.GET, DataFormat.Json, id);
//            var model = JsonConvert.DeserializeObject<ReconcilOrderDetail>(result.Content);
//            ReconcilOrdersAccountsListDDL(id);
//            GetROVersionsDDl(id);
//            model.ReconciliationOrderSummaryData = await GetReconciliationOrderSummary(id) ?? new ReconciliationOrderSummaryData();
//            return View(model ?? new ReconcilOrderDetail());
//        }

//        [HttpGet]
//        public async Task<ActionResult> GetReconcilOrderData(long id)
//        {
//            var result = await ExecuteApi("ReconcilOrders/Details/" + id, Method.GET, DataFormat.Json, id);
//            var model = JsonConvert.DeserializeObject<ReconcilOrderDetail>(result.Content);
//            //return new JsonResponseResult(result.StatusCode, result.Content)
//            //{
//            //    ContentType = "application/json"
//            //};
//            return PartialView("_ROData", model);
//        }

//        [HttpGet]
//        public async Task<ActionResult> GetROSummaryData(long id)
//        {
//            var summaryData = await GetReconciliationOrderSummary(id) ?? new ReconciliationOrderSummaryData();
//            return PartialView("_Summary", summaryData);
//        }

//        [HttpPost]
//        public async Task<ActionResult> AddNewVersion(CreateReconcilOrderVersionInput data)
//        {
//            var result = await ExecuteApi("ReconcilOrders/AddNewVersion/", Method.POST, DataFormat.Json, data);
//            return new JsonResponseResult(result.StatusCode, result.Content);
//        }

//        [HttpPost]
//        public async Task<ActionResult> ReconcilVersion(ReconcilVersionInput data)
//        {
//            var result = await ExecuteApi("ReconOrderVersion/ReconcilVersion/", Method.POST, DataFormat.Json, data);
//            return new JsonResponseResult(result.StatusCode, result.Content);
//        }

//        [HttpGet]
//        public async Task<ActionResult> DetailsAccountChanged(long reconcilOrderId, int accountId)
//        {
//            var result = await ExecuteApi("ReconcilOrders/GetStatementDetails", Method.POST, DataFormat.Json, new GetEntityStatementDetails() { ReconcilOrderId = reconcilOrderId, AccountId = accountId });
//            var model = JsonConvert.DeserializeObject<StatementDataDetails>(result.Content);
//            return PartialView("_AccountDetailsList", model);
//        }

//        [HttpGet]
//        public async Task<ActionResult> GetMatchedTransactions(long versionId)
//        {
//            var result = await ExecuteApi("Comparer/GetMatchedTransactions/" + versionId, Method.GET, DataFormat.Json, versionId);
//            var model = JsonConvert.DeserializeObject<MatchedTransctions>(result.Content);
//            return PartialView("_VersionMatchedTransactions", model);
//        }

//        [HttpGet]
//        public async Task<ActionResult> GetUnMatchedTransactions(long versionId)
//        {
//            var result = await ExecuteApi("Comparer/GetUnMatchedTransactions/" + versionId, Method.GET, DataFormat.Json, versionId);
//            var model = JsonConvert.DeserializeObject<UnMatchedTransactions>(result.Content);
//            return PartialView("_VersionUnMatchedTransactions", model);
//        }

//        [HttpGet]
//        public async Task<ActionResult> GetReconcilOrderActions(long reconcilOrderId)
//        {
//            var result = await ExecuteApi("ReconcilOrders/GetReconcilOrderActions/" + reconcilOrderId, Method.GET, DataFormat.Json, reconcilOrderId);
//            var model = JsonConvert.DeserializeObject<ResultList<ReconOrderActionsData>>(result.Content);
//            return PartialView("_Actions", model.Payload);
//        }

//        [HttpGet]
//        public async Task<ActionResult> GetReconcilOrderVersions(long reconcilOrderId)
//        {
//            var result = await ExecuteApi("ReconcilOrders/GetReconcilOrderVersions/" + reconcilOrderId, Method.GET, DataFormat.Json, reconcilOrderId);
//            var model = JsonConvert.DeserializeObject<List<VersionRoleMappingData>>(result.Content);
//            return PartialView("_VersionRoles", model);
//        }

//        [HttpGet]
//        public async Task<ActionResult> GetVersionResults(long versionId)
//        {
//            var result = await ExecuteApi("ReconcilOrders/GetVersionResults/" + versionId, Method.GET, DataFormat.Json, versionId);
//            var model = JsonConvert.DeserializeObject<List<VersionResultsVM>>(result.Content);
//            return PartialView("_VersionResults", model);
//        }

//        [HttpGet]
//        public async Task<ActionResult> GetReconcilOrderAccounts(long reconcilOrderId)
//        {
//            var result = await ExecuteApi("Accounts/GetAccountsByReconcilOrderId/" + reconcilOrderId, Method.GET, DataFormat.Json, reconcilOrderId);
//            var model = JsonConvert.DeserializeObject<List<ReconcilOrdereAccounts>>(result.Content);
//            return PartialView("_Accounts", model);
//        }

//        #endregion

//        #region Private Methods
//        private async Task<ReconciliationOrderSummaryData> GetReconciliationOrderSummary(long id)
//        {
//            var Summaryresult = await ExecuteApi("ReconcilOrders/GetReconciliationOrderSummary/" + id, Method.GET, DataFormat.Json, id);
//            return JsonConvert.DeserializeObject<ReconciliationOrderSummaryData>(Summaryresult.Content);
//        }

//        private void AccountsListDDL()
//        {
//            var result = ExecuteApi("Accounts", Method.GET, DataFormat.Json, null);
//            var data = JsonConvert.DeserializeObject<List<AccountDto>>(result.Result.Content);
//            var list = new List<SelectListItem>();
//            if (data != null)
//            {
//                foreach (var item in data)
//                {
//                    list.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.AccountName + '-' + item.AccountIBAN + '-' + item.EntityId });
//                }
//            }
//            ViewBag.AccountList = list;
//        }
//        private void ReconcilOrdersAccountsListDDL(long id)
//        {
//            var result = ExecuteApi("ReconcilOrders/GetReconcilOrderAccounts/" + id, Method.GET, DataFormat.Json, id);
//            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AccountDto>>(result.Result.Content);
//            var list = new List<SelectListItem>();
//            if (data != null)
//            {
//                foreach (var item in data)
//                {
//                    list.Add(new SelectListItem() { Value = item.Id.ToString(), Text = item.AccountName + '-' + item.AccountIBAN + '-' + item.EntityId });
//                }
//            }
//            ViewBag.AccountList = list;
//        }

//        private void GetROVersionsDDl(long id)
//        {
//            var result = ExecuteApi("ReconcilOrders/GetReconcilOrderVersions/" + id, Method.GET, DataFormat.Json, id);
//            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ROVersionsDto>>(result.Result.Content);
//            var list = new List<SelectListItem>();
//            if (data != null)
//            {
//                foreach (var item in data)
//                {
//                    list.Add(new SelectListItem() { Value = item.VersionId.ToString(), Text = item.VersionName + "-" + item.RuleName });
//                }
//            }
//            ViewBag.VersionsList = list;
//        }
//        #endregion
//    }
//}
