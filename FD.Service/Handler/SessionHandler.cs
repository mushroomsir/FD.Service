using System.Web.SessionState;
using FD.Service.Model;

namespace FD.Service.Handler
{
    internal class SessionHandler : ServiceHandler, IRequiresSessionState
    {
        public SessionHandler(ServiceInfo info) : base(info)
        {
           
        }
    }
}
