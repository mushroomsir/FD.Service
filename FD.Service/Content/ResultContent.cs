using System;
using System.Collections.Generic;
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

    public class ActionAfterContent : ActionBeforeContent
    {
        public ActionAfterContent(IDictionary<string, object> parameters, HttpContext hc, string message, object result)
            : base(parameters, hc, message)
        {
            Result = result;
        }
        public object Result { get; set; }
    }
    public class ResultBeforeContent : ActionAfterContent
    {
        public ResultBeforeContent(IDictionary<string, object> parameters, HttpContext hc, string message, object result)
            : base(parameters, hc, message, result)
        {
        }
    }
    public class ResultAfterContent : ActionAfterContent
    {
        public ResultAfterContent(IDictionary<string, object> parameters, HttpContext hc, string message, object result)
            : base(parameters, hc, message, result)
        {
        }
    }
   
}
