using System;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using FD.Service.Model;

namespace FD.Service
{
    internal static class ServiceExecutor
    {
        internal static void ProcessRequest(HttpContext context, ServiceInfo info)
        {
            if (string.IsNullOrEmpty(context.Request.ContentType))
                context.Response.AddHeader("Content-Type", "text/html; charset=utf-8");
            else
                context.Response.ContentType = context.Request.ContentType;
            try
            {
                var filters = GetFilters(info);
                FiltersInvoker.OnControllerBefore(context, filters);

                var paramslist = StructureParams(context, info);
                var methodInfo = info.InvokeInfo.MethodAttrInfo.MethodInfo;
                FiltersInvoker.OnActionBefore(context, filters, paramslist);
                object result = null;

                result = CreateInvokeDelegate(methodInfo)
                    .Invoke(info.InvokeInfo.ServiceInstance, paramslist.Select(n => n.Value).ToArray());

                FiltersInvoker.OnActionAfter(context, filters, paramslist, result);
                var renderResult = BuildResult(info, result);
                context.Response.Write(renderResult);
            }
            catch (HttpResponseException exception)
            {
                context.Response.StatusCode = (int) exception.Response.StatusCode;
                context.Response.ContentEncoding = exception.Response.StringContent.Encoding ?? Encoding.Default;
                context.Response.Write(exception.Response.StringContent.Content);
            }
            catch (Exception ex)
            {
                var exception = info.InvokeInfo.MethodAttrInfo.ExceptionFilters;
                if (exception != null && exception.Any())
                {
                    FiltersInvoker.OnException(context, exception, ex);
                }
                else
                {
                    var sb=new StringBuilder(255);
                    sb.Append("<Error><Message>");
                    sb.Append(HttpUtility.HtmlEncode(ex.Message));
                    sb.Append("</Message><ExceptionType>");
                    sb.Append(HttpUtility.HtmlEncode(ex.GetType()));
                    sb.Append("</ExceptionType><StackTrace>");
                    sb.Append(HttpUtility.HtmlEncode(ex.StackTrace));
                    sb.Append("</StackTrace></Error>");
                    context.Response.ContentType = "text/xml";
                    context.Response.StatusCode = 500;
                    context.Response.Write(sb.ToString());
                }
            }

        }
        internal static IEnumerable<FdFilterAttribute> GetFilters(ServiceInfo info)
        {
           return info.InvokeInfo.MethodAttrInfo.FiltersAttr.OrderBy(n => n.Order);
        }
        private static Func<object, object[], object> CreateInvokeDelegate(MethodInfo methodInfo)
        {
            var instanceParameter = Expression.Parameter(typeof(object), "instance");
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

            var parameterExpressions = new List<Expression>();
            var paramInfos = methodInfo.GetParameters();
            for (var i = 0; i < paramInfos.Length; i++)
            {
                var valueObj = Expression.ArrayIndex(
                    parametersParameter, Expression.Constant(i));
                var valueCast = Expression.Convert(
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

        private static object BuildResult(ServiceInfo info, object result)
        {
            var methodAttrInfo = info.InvokeInfo.MethodAttrInfo;

            if (methodAttrInfo.MethodInfo.ReturnType.IsEnum)
            {
                result = (int) result;
            }
            else if (methodAttrInfo.MethodAttr != null && methodAttrInfo.MethodAttr.ResponseFormat == ResponseFormat.Json)
            {
                result = new JavaScriptSerializer().Serialize(result);
            }
            return result;
        }

        private static IDictionary<string,object> StructureParams(HttpContext context, ServiceInfo info)
        {
            var destType = info.InvokeInfo.MethodAttrInfo.Parameters;
            var paramslist =new Dictionary<string,object>();
            foreach (var t in destType)
            {
                Type type = t.ParameterType;
                if (type == typeof(HttpRequest))
                {
                    paramslist.Add(t.Name,context.Request);
                    continue;
                }
                else if (type == typeof(HttpContext))
                {
                    paramslist.Add(t.Name, context);
                    continue;
                }
                var values = context.Request[t.Name];
                if (values == null)
                    throw new ArgumentNullException(t.Name);

                paramslist.Add(t.Name, Convert.ChangeType(values, type));
            }
            return paramslist;
        }
        
    }
}
