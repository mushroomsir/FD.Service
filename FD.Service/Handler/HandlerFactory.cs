using System;
using System.Text.RegularExpressions;
using System.Web;
using FD.Service.Model;

namespace FD.Service.Handler
{
    internal class HandlerFactory : IHttpHandlerFactory
    {
        public Match Match { get; set; }
        public virtual IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated)
        {
            var np = new NamesPair
            {
                ServiceName = Match.Groups["name"].Value,
                MethodName = Match.Groups["method"].Value
            };

            if (string.IsNullOrEmpty(np.ServiceName) || string.IsNullOrEmpty(np.MethodName))
                ExceptionHelper.Throw404Exception(context);

            var vInfo = ReflectionHelper.GetInvokeInfo(np);
            if (vInfo == null)
                ExceptionHelper.Throw404Exception(context);

            var sinfo = new ServiceInfo(np, vInfo);
            if (sinfo.InvokeInfo == null)
                ExceptionHelper.Throw404Exception(context);
            
            switch (vInfo.ServiceTypeInfo.ServiceAttr.SessionMode)
            {
                case SessionMode.NotSupport:
                    return new ServiceHandler(sinfo);
                case SessionMode.Support:
                    return new SessionHandler(sinfo);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        public virtual void ReleaseHandler(IHttpHandler handler)
        {

        }
    }
}
