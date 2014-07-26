using System.Web;
using FD.Service.Model;

namespace FD.Service.Handler
{
    internal class ServiceHandler : IHttpHandler
    {
        private ServiceInfo Info { get; set; }
        public ServiceHandler(ServiceInfo info)
        {
            Info = info;
        }
        public void ProcessRequest(HttpContext context)
        {
            ServiceExecutor.ProcessRequest(context, Info);
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
