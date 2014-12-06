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