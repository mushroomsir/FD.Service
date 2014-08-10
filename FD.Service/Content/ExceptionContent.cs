using System;
using System.Collections.Generic;
using System.Web;

namespace FD.Service
{
    public class ExceptionContent : FdContent
    {
        public ExceptionContent(HttpContext hc, string message, Exception ex)
            : base(hc, message)
        {
            Exception = ex;
        }
        public Exception Exception { get; set; }
    }
}
