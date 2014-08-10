using System;

namespace FD.Service
{
	[AttributeUsage(AttributeTargets.Method,AllowMultiple=false,Inherited=false)]
    public class FdMethodAttribute : System.Attribute
	{
		public ResponseFormat ResponseFormat { get; set; }
	}
	
}
