using System;
using System.Text.RegularExpressions;
using System.Web;

namespace FD.Service
{
    internal class UrlRoutingModule : IHttpModule
    {
        static HandlerFactory hf = new HandlerFactory();
        public void Init(HttpApplication app)
        {
            app.PostResolveRequestCache += new EventHandler(app_PostResolveRequestCache);
        }
        private void app_PostResolveRequestCache(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;

            Match match = Regex.Match(app.Context.Request.Path, ServiceHandler.RouteAddress);
            if (match.Success)
                app.Context.RemapHandler(hf.GetHandler(app.Context, app.Request.RequestType, null, null));
        }
        public void Dispose()
        {

        }
    }
}
