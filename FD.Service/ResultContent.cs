using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace FD.Service
{
    public class FdContent
    {
        public FdContent(HttpContext hc, string message)
        {
            Context = hc;
            Message = message;
        }
        public HttpContext Context { get; set; }
        public string Message { get; set; }
    }
    public class ControllerBeforeContent : FdContent
    {
        public ControllerBeforeContent(HttpContext hc, string message)
            : base(hc, message)
        {

        }
    }
    public class ActionBeforeContent : ControllerBeforeContent
    {
        public ActionBeforeContent(IDictionary<string, object> parameters, HttpContext hc, string message)
            : base(hc, message)
        {
            this.Parameters = parameters;
        }
        public IDictionary<string, object> Parameters { get; set; }
    }
    public class ActionExceptionContent : ActionBeforeContent
    {
        public ActionExceptionContent(IDictionary<string, object> parameters, HttpContext hc, string message, Exception ex)
            : base(parameters, hc, message)
        {
            Exception = ex;
        }
        public Exception Exception { get; set; }
    }

    public class ActionAfterContent : ActionBeforeContent
    {
        public ActionAfterContent(IDictionary<string, object> parameters, HttpContext hc, string message, object result)
            : base(parameters, hc, message)
        {
            Result = result;
        }
        public object Result { get; set; }
    }
    public class ActionExcutedContent : ActionAfterContent
    {
        public ActionExcutedContent(IDictionary<string, object> parameters, HttpContext hc, string message, object Result)
            : base(parameters, hc, message, Result)
        {
        }
    }
   
}
