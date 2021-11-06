using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ServerSidePagination.Models
{
    public class ServicesLogger
    {
        public long Id { get; set; }
        public string ServiceData { get; set; }
        public int LogType { get; set; }
        public int? ServiceID { get; set; }
        public long? ThreadID { get; set; }
        public string SourceIp { get; set; }
        public string DestinationIP { get; set; }
        public string URL { get; set; }
        public string HttpVerb { get; set; }
        public string QueryString { get; set; }
        public string StatusCode { get; set; }
        public Guid ReferenceId { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public DateTime? CreationDate { get; set; }

    }
}