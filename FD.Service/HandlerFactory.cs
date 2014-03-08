using System;
using System.Web;

namespace FD.Service
{
    public class HandlerFactory : IHttpHandlerFactory
    {
        public virtual IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            if (context.Request.AppRelativeCurrentExecutionFilePath.EndsWith(".ashx", StringComparison.OrdinalIgnoreCase) == true)
                return new ServiceHandler2();
            else if (context.Request.AppRelativeCurrentExecutionFilePath.EndsWith(".mx", StringComparison.OrdinalIgnoreCase) == true)
                return new ServiceHandler();
            else
                return new ServiceHandler();
        }
        public virtual void ReleaseHandler(IHttpHandler handler)
        {

        }
    }
}
