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
        private static List<TypeAndAttrInfo> TypeList = new List<TypeAndAttrInfo>(256);
        private static Hashtable MethodTable = Hashtable.Synchronized(new Hashtable(4096, StringComparer.OrdinalIgnoreCase));

        static ReflectionHelper()
        {
            InitServiceTypes();
        }

        private static void InitServiceTypes()
        {
            var assemblies = BuildManager.GetReferencedAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                if (assembly.FullName.StartsWith("System.", StringComparison.OrdinalIgnoreCase))
                    continue;

                FoundFdService(assembly);
            }
        }

        private static void FoundFdService(Assembly assembly)
        {
            try
            {
                var typeList = from t in assembly.GetExportedTypes()
                               let a = t.GetCustomAttributes(typeof(FdServiceAttribute), false) as FdServiceAttribute[]
                               let b = t.GetCustomAttributes(typeof(FdFilterAttribute), true) as FdFilterAttribute[]
                               where a.Length > 0
                               select new TypeAndAttrInfo
                               {
                                   ServiceType = t,
                                   ServiceAttr = a[0],
                                   FiltersAttr = b
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
            if (pair == null)
                throw new ArgumentNullException("pair");

            InvokeInfo vkInfo = new InvokeInfo();

            vkInfo.ServiceTypeInfo = GetServiceType(pair.ServiceName);
            if (vkInfo.ServiceTypeInfo == null)
                return null;

            vkInfo.MethodAttrInfo = GetServiceMethod(vkInfo.ServiceTypeInfo.ServiceType, pair.MethodName);
            if (vkInfo.MethodAttrInfo == null)
                return null;


            if (vkInfo.MethodAttrInfo.MethodInfo.IsStatic == false)
                vkInfo.ServiceInstance = Activator.CreateInstance(vkInfo.ServiceTypeInfo.ServiceType);

            return vkInfo;
        }
        private static TypeAndAttrInfo GetServiceType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
                throw new ArgumentNullException("typeName");



            if (typeName.IndexOf('.') > 0)
                return TypeList.FirstOrDefault(t => string.Compare(t.ServiceType.FullName, typeName, true) == 0);
            else
                return TypeList.FirstOrDefault(t => string.Compare(t.ServiceType.Name, typeName, true) == 0);
        }

        private static MethodAndAttrInfo GetServiceMethod(Type type, string methodName)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException("methodName");

            string key = methodName + "@" + type.FullName;
            MethodAndAttrInfo mi = MethodTable[key] as MethodAndAttrInfo;

            if (mi != null)
                return mi;

            MethodInfo method = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

            if (method == null)
                return null;

            var attrs = method.GetCustomAttributes(typeof(FdMethodAttribute), false) as FdMethodAttribute[];

            var filters = method.GetCustomAttributes(typeof(FdFilterAttribute), true) as FdFilterAttribute[];

            mi = new MethodAndAttrInfo
            {
                MethodInfo = method,
                Parameters = method.GetParameters(),
                MethodAttr = attrs[0],
                FiltersAttr = filters
            };

            MethodTable[key] = mi;

            return mi;
        }
    }
}
