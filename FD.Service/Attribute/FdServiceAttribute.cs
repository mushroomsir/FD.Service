﻿using System;

namespace FD.Service
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class FdServiceAttribute : System.Attribute
	{
		public  SessionMode SessionMode { get; set; }
	    public  bool IsPublicAllMethod { get; set; }
	}
}
