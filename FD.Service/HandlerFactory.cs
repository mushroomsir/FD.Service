using System;
using System.Web;

namespace FD.Service
{
    public class HandlerFactory : IHttpHandlerFactory
    {
        public virtual IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            return new ServiceHandler();
        }
        public virtual void ReleaseHandler(IHttpHandler handler)
        {

        }
    }
}
