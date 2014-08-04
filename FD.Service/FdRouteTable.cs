using System;
using System.Collections.Generic;
using System.Configuration;

namespace FD.Service
{
    public static class FdRouteTable
    {
        private static readonly string RouteFolder = "api";
        private const string AjaxPattern = @"(?<name>\w+)/(?<method>\w+)";
        internal static string RouteAddress { get; private set; }
       
        internal static string[] AssemblyList { get; private set; }
        static FdRouteTable()
        {
            var fdservice = ConfigurationManager.AppSettings["fdservice"];
            if (fdservice != null)
                RouteFolder = fdservice;
            
            RouteAddress = string.Format("/{0}/{1}", RouteFolder, AjaxPattern);
        }
        static void RegisterRoute(string name, string url)
        {
            
        }
        public static void RegisterService(params string[] assemblyName)
        {
            AssemblyList = assemblyName;
        }
    }
}
