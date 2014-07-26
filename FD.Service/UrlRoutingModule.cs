using System;
using System.Text.RegularExpressions;
using System.Web;
using FD.Service.Handler;

namespace FD.Service
{
    internal class UrlRoutingModule : IHttpModule
    {
        static readonly HandlerFactory Hf = new HandlerFactory();
        public void Init(HttpApplication app)
        {
            app.PostResolveRequestCache += new EventHandler(app_PostResolveRequestCache);
        }

        private void app_PostResolveRequestCache(object sender, EventArgs e)
        {
            var app = (HttpApplication) sender;

            var match = Regex.Match(app.Context.Request.Path, FdRouteTable.RouteAddress);
            if (!match.Success)
                return;
            Hf.Match = match;
            var handler = Hf.GetHandler(app.Context, app.Request.RequestType, null, null);
            app.Context.RemapHandler(handler);
        }

        public void Dispose()
        {
             
        }
    }
}
