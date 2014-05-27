using System;
using System.Reflection;

namespace FD.Service
{
	internal class ServiceInfo
	{
		public ServiceInfo(NamesPair pair, InvokeInfo vkInfo)
		{
			if (pair == null)
				throw new ArgumentNullException("pair");
			if (vkInfo == null)
				throw new ArgumentNullException("vkInfo");

			this.NamesPair = pair;
			this.InvokeInfo = vkInfo;
		}

		public NamesPair NamesPair { get; private set; }
		public InvokeInfo InvokeInfo { get; private set; }
	}
	
	public class NamesPair
	{
		
		public string ServiceName { get; set; }
		
		public string MethodName { get; set; }
	}
	internal class InvokeInfo
	{
		public TypeAndAttrInfo ServiceTypeInfo { get; set; }
		public MethodAndAttrInfo MethodAttrInfo { get; set; }
		public object ServiceInstance { get; set; }
	}
	internal class TypeAndAttrInfo
	{
		public Type ServiceType { get; set; }
		public FdServiceAttribute Attr { get; set; }
	}
	
	internal class MethodAndAttrInfo
	{
		public MethodInfo MethodInfo { get; set; }
		public FdMethodAttribute Attr { get; set; }
		public ParameterInfo[] Parameters { get; set; }
	}
}
