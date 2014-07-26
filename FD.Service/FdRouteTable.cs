using System.Configuration;

namespace FD.Service
{
    internal class FdRouteTable
    {
        private static readonly string RouteFolder = "api";
        private const string AjaxPattern = @"(?<name>\w+)/(?<method>\w+)";

        internal static string RouteAddress { get; private set; }

        static FdRouteTable()
        {
            var fdservice = ConfigurationManager.AppSettings["fdservice"];
            if (fdservice != null)
                RouteFolder = fdservice;
            
            RouteAddress = string.Format("/{0}/{1}", RouteFolder, AjaxPattern);
        }
    }
}
