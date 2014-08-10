using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FD.Service;
using FD.Service.Attribute;

namespace FdServiceTest.api
{
    public class ErrorAttribute : FdFilterAttribute,IExceptionFilter
    {
        public void OnException(ExceptionContent filterContext)
        {
            filterContext.Context.Response.Write(filterContext.Exception.Message);
        }
    }
}