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
		private static List<TypeAndAttrInfo> s_typeList;

		static ReflectionHelper()
		{
			InitServiceTypes();
		}

		
		private static void InitServiceTypes()
		{
			s_typeList = new List<TypeAndAttrInfo>(256);

			ICollection assemblies = BuildManager.GetReferencedAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				if (assembly.FullName.StartsWith("System.", StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}
				try
				{
					(from t in assembly.GetExportedTypes()
					 let a = t.GetCustomAttributes(typeof(FdServiceAttribute), false) as FdServiceAttribute[]
					 where a.Length > 0
					 select new TypeAndAttrInfo
					 {
						 ServiceType = t,
						 Attr = a[0],
					 }
					 ).ToList().ForEach(b => s_typeList.Add(b));
				}
				catch { }
			}
		}

		
		/// <summary>
		/// 获取参数构造信息
		/// </summary>
		/// <param name="pair"></param>
		/// <returns></returns>
		public static InvokeInfo GetInvokeInfo(NamesPair pair)
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
		#region  信息获取
		
		private static TypeAndAttrInfo GetServiceType(string typeName)
		{
			if (string.IsNullOrEmpty(typeName))
				throw new ArgumentNullException("typeName");


			
			if (typeName.IndexOf('.') > 0)
				return s_typeList.FirstOrDefault(t => string.Compare(t.ServiceType.FullName, typeName, true) == 0);
			else
				return s_typeList.FirstOrDefault(t => string.Compare(t.ServiceType.Name, typeName, true) == 0);
		}



		private static Hashtable s_methodTable = Hashtable.Synchronized(new Hashtable(4096, StringComparer.OrdinalIgnoreCase));

	
		private static MethodAndAttrInfo GetServiceMethod(Type type, string methodName)
		{
			if (type == null)
				throw new ArgumentNullException("type");
			if (string.IsNullOrEmpty(methodName))
				throw new ArgumentNullException("methodName");

			
			string key = methodName + "@" + type.FullName;
			MethodAndAttrInfo mi = (MethodAndAttrInfo)s_methodTable[key];

			if (mi == null)
			{
				
				MethodInfo method = type.GetMethod(methodName,
						 BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase);

				if (method == null)
					return null;

				FdMethodAttribute[] attrs = method.GetCustomAttributes(typeof(FdMethodAttribute), false) as FdMethodAttribute[];
				if (attrs.Length != 1)
					return null;

				mi = new MethodAndAttrInfo
				{
					MethodInfo = method,
					Parameters = method.GetParameters(),
					Attr = attrs[0]
				};

				s_methodTable[key] = mi;
			}

			return mi;
		}
		#endregion 
	}
}
