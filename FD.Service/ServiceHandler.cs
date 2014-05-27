using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Configuration;
namespace FD.Service
{
    internal class ServiceHandler : IHttpHandler
    {
        internal static string AjaxPattern = @"(?<name>\w+)/(?<method>\w+)";
        public static string RouteFolder = "api";
        internal static string RouteAddress;
        static ServiceHandler()
        {
            var fdservice = ConfigurationManager.AppSettings["fdservice"];
            if (fdservice != null)
            {
                RouteFolder = fdservice;
            }
            RouteAddress = string.Format("/{0}/{1}", RouteFolder, AjaxPattern);
        }

        public void ProcessRequest(HttpContext context)
        {
            Match match = Regex.Match(context.Request.Path, RouteAddress);
            if (match.Success == false)
                throw new ArgumentNullException("info");

            NamesPair np = new NamesPair
            {
                ServiceName = match.Groups["name"].Value,
                MethodName = match.Groups["method"].Value
            };

            if (string.IsNullOrEmpty(np.ServiceName) || string.IsNullOrEmpty(np.MethodName))
                ExceptionHelper.Throw404Exception(context);

            InvokeInfo vkInfo = ReflectionHelper.GetInvokeInfo(np);
            if (vkInfo == null)
                ExceptionHelper.Throw404Exception(context);


            ServiceInfo info = new ServiceInfo(np, vkInfo);
            ServiceExecutor.ProcessRequest(context, info);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
