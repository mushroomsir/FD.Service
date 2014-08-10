using FD.Service.Attribute;
using FD.Service.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;

namespace FD.Service
{
    internal class ReflectionHelper
    {
        private static readonly List<TypeAndAttrInfo> TypeList = new List<TypeAndAttrInfo>(256);

        private static readonly Hashtable MethodTable =
            Hashtable.Synchronized(new Hashtable(4096, StringComparer.OrdinalIgnoreCase));

        static ReflectionHelper()
        {
            InitServiceTypes();
        }

        private static void InitServiceTypes()
        {
            var assemblies = BuildManager.GetReferencedAssemblies();
            if (FdRouteTable.AssemblyList != null)
            {
                foreach (var item in FdRouteTable.AssemblyList)
                {
                    var item1 = item;
                    var result =
                        assemblies.Cast<Assembly>()
                            .Where(n => String.Compare(n.GetName().Name, item1, StringComparison.OrdinalIgnoreCase) == 0);
                    if (result.Any())
                    {
                        FoundFdService(result.First());
                    }
                }
            }
            else
            {
                var result =
                    assemblies.Cast<Assembly>()
                        .Where(assembly => !assembly.FullName.StartsWith("System.", StringComparison.OrdinalIgnoreCase));
                foreach (var assembly in result)
                {
                    FoundFdService(assembly);
                }
            }
        }

        private static void FoundFdService(Assembly assembly)
        {
            try
            {
                var typeList = from t in assembly.GetExportedTypes()
                    let a = t.GetCustomAttributes(typeof (FdServiceAttribute), false) as FdServiceAttribute[]
                    let b = t.GetCustomAttributes(typeof (FdFilterAttribute), true) as FdFilterAttribute[]
                    where a.Length > 0
                    select new TypeAndAttrInfo
                    {
                        ServiceType = t,
                        ServiceAttr = a[0],
                        FiltersAttr = b,

                    };

                foreach (var item in typeList)
                {
                    TypeList.Add(item);
                }

            }
            catch (Exception ex)
            {
                ExceptionHelper.ThrowNotFoundService(ex);
            }
        }

        internal static InvokeInfo GetInvokeInfo(NamesPair pair)
        {
            var vkInfo = new InvokeInfo
            {
                ServiceTypeInfo =
                    TypeList.FirstOrDefault(
                        t =>
                            String.Compare(t.ServiceType.Name, pair.ServiceName, StringComparison.OrdinalIgnoreCase) ==
                            0)
            };

            if (vkInfo.ServiceTypeInfo == null)
                return null;

            vkInfo.MethodAttrInfo = GetServiceMethod(vkInfo.ServiceTypeInfo, pair.MethodName);
            if (vkInfo.MethodAttrInfo == null)
                return null;


            if (vkInfo.MethodAttrInfo.MethodInfo.IsStatic == false)
                vkInfo.ServiceInstance = Activator.CreateInstance(vkInfo.ServiceTypeInfo.ServiceType);

            return vkInfo;
        }


        private static MethodAndAttrInfo GetServiceMethod(TypeAndAttrInfo type, string methodName)
        {
            var key = methodName + "@" + type.ServiceType.FullName;
            var mi = MethodTable[key] as MethodAndAttrInfo;

            if (mi != null)
                return mi;

            var method = type.ServiceType.GetMethod(methodName,
                BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (method == null)
                return null;
            var attrs = method.GetCustomAttributes(typeof (FdMethodAttribute), false) as FdMethodAttribute[];
            if (attrs.Length < 1 && type.ServiceAttr.IsPublicAllMethod == false)
                return null;

            FdMethodAttribute methodAttr = null;
            if (attrs.Length > 0)
                methodAttr = attrs[0];

            var filters = method.GetCustomAttributes(typeof (FdFilterAttribute), true) as FdFilterAttribute[];

            IEnumerable<IExceptionFilter> resFilters = null;
            if (filters.Length > 0)
                resFilters = filters.Where(n => n is IExceptionFilter).Cast<IExceptionFilter>();

            mi = new MethodAndAttrInfo
            {
                MethodInfo = method,
                Parameters = method.GetParameters(),
                MethodAttr = methodAttr,
                FiltersAttr = filters,
                ExceptionFilters = resFilters

            };

            MethodTable[key] = mi;

            return mi;
        }
    }
}
