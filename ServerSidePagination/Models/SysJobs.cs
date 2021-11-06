using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSidePagination.Models
{
    public class SysJob
    {
        public long Id { get; set; }
        public string JobName { get; set; }
        public int LUTJobTypeId { get; set; }
        public DateTime CreateDate { get; set; }
        public long? ParentId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int LUTJobStatusId { get; set; }
        public long ReconOrderId { get; set; }
        public string JobData { get; private set; }

        public LUTJobType JobType { get; set; }
        public JobDetail JobDetail { get; set; }

        public void AddJobData(JobData obj)
        {
            JobData = JsonConvert.SerializeObject(obj);
        }
        public JobData ReadJobData()
        {
            return JsonConvert.DeserializeObject<JobData>(JobData);
        }

        public void Start()
        {
            LUTJobStatusId = (int)EnumJobStatus.Running;
            JobDetail.LUTJobStatusId = (int)EnumJobStatus.Running;
            JobDetail.SystemMessage = "تم بدء التنفيذ";
            StartDate = DateTime.Now;
        }
        public void Success(string msg = "تم التنفيذ بنجاح")
        {
            LUTJobStatusId = (int)EnumJobStatus.Success;
            JobDetail.LUTJobStatusId = (int)EnumJobStatus.Success;
            JobDetail.SystemMessage = msg;
            EndDate = DateTime.Now;
        }
        public void Failed(string msg, string stackTrace = null)
        {
            LUTJobStatusId = (int)EnumJobStatus.Failed;
            JobDetail.LUTJobStatusId = (int)EnumJobStatus.Failed;
            JobDetail.SystemMessage = msg;
            JobDetail.Exception = stackTrace;
        }
    }
    public class JobData
    {
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public List<KeyValuePair<long, string>> ROAccounts { get; set; }
    }
    public class JobDetail 
    {
        public long Id { get; set; }
        public long JobId { get; set; }
        public DateTime CreateDate { get; set; }
        public string SystemMessage { get; set; }
        public string Exception { get; set; }
        public int LUTJobStatusId { get; set; }
        public string JobParamters { get; set; }
        public SysJob SysJob { get; set; }

        //public void AddJobParams(params KeyValuePair<string, object>[] parameters)
        //{
        //foreach (KeyValuePair<string, object> param in parameters)
        //{
        //    JobParamters += $"{param.Key}:{param.Value},";
        //}
        //JobParamters.Remove(JobParamters.Length - 1, 1);
        //}
    }
    public class LUTJobType 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
    public enum EnumJobStatus
    {
        New = 1,
        Success = 2,
        Failed = 3,
        Running = 4
    }
}