using System;
using System.Linq;
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
            app.PostResolveRequestCache += app_PostResolveRequestCache;
        }

        private void app_PostResolveRequestCache(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;

            var match = FdRouteTable.RouteTable.Select(item => Regex.Match(app.Context.Request.Path, item, RegexOptions.IgnoreCase)).FirstOrDefault(temp => temp.Success);
            if (match == null)
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
