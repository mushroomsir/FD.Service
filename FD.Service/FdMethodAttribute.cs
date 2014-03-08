using System;

namespace FD.Service
{
	[AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=false)]
	public class FdMethodAttribute:Attribute
	{
		public ResponseFormat ResponseFormat { get; set; }
	}
	public enum ResponseFormat
	{
		None = 0,
		Json = 1,
		//Xml = 2,
	}
}
