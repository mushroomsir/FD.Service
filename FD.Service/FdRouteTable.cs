using System.Collections.Generic;

namespace FD.Service
{
    public static class FdRouteTable
    {
        internal static string[] AssemblyList { get; private set; }

        internal static List<string> RouteTable { get; private set; }
        static FdRouteTable()
        {
            RouteTable = new List<string>();
        }
        /// <summary>
        /// Register route for API request
        /// </summary>
        /// <param name="name"></param>
        /// <param name="url"></param>
        public static void RegisterRoute(string name, string url)
        {
            var u = url.Replace("{controller}", @"(?<name>\w+)").Replace("{action}", @"(?<method>\w+)");
            RouteTable.Add(u);
        }
        /// <summary>
        /// Register Service.
        /// </summary>
        /// <param name="assemblyName">Assembly Name</param>
        public static void RegisterService(params string[] assemblyName)
        {
            AssemblyList = assemblyName;

            ReflectionHelper.InitServiceTypes();
        }
    }
}
