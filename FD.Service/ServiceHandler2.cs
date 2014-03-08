﻿using System;
using System.Text.RegularExpressions;
using System.Web;

namespace FD.Service
{
	internal class ServiceHandler2 : IHttpHandler
	{
		internal static readonly string AjaxPattern = @"/(?<name>\w+)[/\.](?<method>\w+)\.ashx";
		public void ProcessRequest(HttpContext context) 
		{
			Match match = Regex.Match(context.Request.Path, AjaxPattern);
			if (match.Success == false)
				throw new ArgumentNullException("info");
				
			NamesPair np =new NamesPair
			{
				ServiceName = match.Groups["name"].Value,
				MethodName = match.Groups["method"].Value
			};
			
			if (string.IsNullOrEmpty(np.ServiceName) || string.IsNullOrEmpty(np.MethodName))
			{
				ExceptionHelper.Throw404Exception(context);
			}

			InvokeInfo vkInfo = ReflectionHelper.GetInvokeInfo(np);
			if (vkInfo == null)
			{
				ExceptionHelper.Throw404Exception(context);
			}

			ServiceInfo info = new ServiceInfo(np, vkInfo);

			ServiceExecutor.ProcessRequest(context, info);
		}
		public bool IsReusable 
		{
			get 
			{ 
				return false; 
			} 
		}
	}
}
