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
	/// <summary>
	/// 包含要调用的服务类型名称和方法名称的一个值对。
	/// </summary>
	public class NamesPair
	{
		/// <summary>
		/// 要调用的服务类名
		/// </summary>
		public string ServiceName { get; set; }
		/// <summary>
		/// 要调用的服务方法名
		/// </summary>
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
	/// <summary>
	/// 调用方法的信息
	/// </summary>
	internal class MethodAndAttrInfo
	{
		public MethodInfo MethodInfo { get; set; }
		public FdMethodAttribute Attr { get; set; }
		public ParameterInfo[] Parameters { get; set; }
	}
}
