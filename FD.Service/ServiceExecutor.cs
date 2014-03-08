using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Collections.Generic;
using System.Web.Script.Serialization;
namespace FD.Service
{

	public static class ServiceExecutor
	{
        internal static void ProcessRequest(HttpContext context, ServiceInfo info)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (info == null || info.InvokeInfo == null)
            {
                throw new SystemException("url错误");
            }
            //参数构造
            var destType = info.InvokeInfo.MethodAttrInfo.Parameters;
            var paramslist = new object[destType.Length];
            for (var i = 0; i < paramslist.Length; i++)
            {
                Type type = destType[i].ParameterType;
                if (type == typeof(HttpRequest))
                {
                    paramslist[i] = context.Request;
                    continue;
                }
                var values = context.Request[destType[i].Name];
                if (values == null)
                    throw new ArgumentNullException(destType[i].Name);

                paramslist[i] = Convert.ChangeType(values, type);

            }
            context.Response.AddHeader("Content-Type", "text/html; charset=utf-8");
            object result = null;
            try
            {
                result = CreateInvokeDelegate(info.InvokeInfo.MethodAttrInfo.MethodInfo)
                   .Invoke(info.InvokeInfo.ServiceInstance, paramslist);
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message);
            }
            if (info.InvokeInfo.MethodAttrInfo.MethodInfo.ReturnType.IsClass)
            {
                result = new JavaScriptSerializer().Serialize(result);
            }
            else if (info.InvokeInfo.MethodAttrInfo.MethodInfo.ReturnType.IsEnum)
            {
                result = (int)result;
            }
            else if (info.InvokeInfo.MethodAttrInfo.Attr.ResponseFormat == ResponseFormat.Json)
            {
                result = new JavaScriptSerializer().Serialize(result);
            }
            context.Response.Write(result);
        }
		/// <summary>
		/// 构建调用委托
		/// </summary>
		/// <param name="methodInfo"></param>
		/// <returns></returns>
		private static Func<object, object[], object> CreateInvokeDelegate(MethodInfo methodInfo)
		{
			var instanceParameter = Expression.Parameter(typeof(object), "instance");
			var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

			var parameterExpressions = new List<Expression>();
			var paramInfos = methodInfo.GetParameters();
			for (int i = 0; i < paramInfos.Length; i++)
			{
				BinaryExpression valueObj = Expression.ArrayIndex(
					parametersParameter, Expression.Constant(i));
				UnaryExpression valueCast = Expression.Convert(
					valueObj, paramInfos[i].ParameterType);

				parameterExpressions.Add(valueCast);
			}

			var instanceCast = methodInfo.IsStatic ? null :
				Expression.Convert(instanceParameter, methodInfo.ReflectedType);

			var methodCall = Expression.Call(instanceCast, methodInfo, parameterExpressions);

			if (methodCall.Type == typeof(void))
			{
				var lambda = Expression.Lambda<Action<object, object[]>>(
						methodCall, instanceParameter, parametersParameter);

				Action<object, object[]> execute = lambda.Compile();
				return (instance, parameters) =>
				{
					execute(instance, parameters);
					return null;
				};
			}
			else
			{
				var castMethodCall = Expression.Convert(methodCall, typeof(object));
				var lambda = Expression.Lambda<Func<object, object[], object>>(
					castMethodCall, instanceParameter, parametersParameter);

				return lambda.Compile();
			}
		}
	}
}
