using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
namespace FD.Service
{

    public static class ServiceExecutor
    {
        internal static void ProcessRequest(HttpContext context, ServiceInfo info)
        {
            if (context == null)
                throw new ArgumentNullException("context");
            if (info == null || info.InvokeInfo == null)
                throw new SystemException("Url Error");

            ControllerExcute(context, info);

            if (string.IsNullOrEmpty(context.Request.ContentType))
                context.Response.AddHeader("Content-Type", "text/html; charset=utf-8");
            else
                context.Response.ContentType = context.Request.ContentType;

            ActionExcute(context, info);
        }
        private static void ControllerExcute(HttpContext context, ServiceInfo info)
        {
            var filters = info.InvokeInfo.ServiceTypeInfo.FiltersAttr.OrderBy(n => n.Order);
            if (filters.Any())
            {
                ControllerBeforeContent rc;
                foreach (var item in filters)
                {
                    rc = new ControllerBeforeContent(context,item.Message);
                    item.OnControllerBefore(rc);
                }
            }
        }
        private static void ActionExcute(HttpContext context, ServiceInfo info)
        {
            var paramslist = StructureParams(context, info);
            object result = null;
            var methodInfo = info.InvokeInfo.MethodAttrInfo.MethodInfo;
            var filters = info.InvokeInfo.MethodAttrInfo.FiltersAttr.OrderBy(n => n.Order);
            try
            {
                if (filters.Any())
                {
                    ActionBeforeContent rc;
                    foreach (var item in filters)
                    {
                        rc = new ActionBeforeContent(paramslist,context,item.Message);
                        item.OnActionBefore(rc);
                    }
                }

                result = CreateInvokeDelegate(methodInfo)
                   .Invoke(info.InvokeInfo.ServiceInstance, paramslist.Select(n => n.Value).ToArray());
            }
            catch (Exception ex)
            {
                if (filters.Any())
                {
                    ActionExceptionContent rc;
                    foreach (var item in filters)
                    {
                        rc = new ActionExceptionContent(paramslist,context,item.Message,ex);
                        item.OnExceptionExecuting(rc);
                    }
                }
            }
            result = BuildResult(info, result);
            if (filters.Any())
            {
                ActionAfterContent rc;
                foreach (var item in filters)
                {
                    rc = new ActionAfterContent(paramslist,context,item.Message,result);
                    item.OnActionAfter(rc);
                }
            }
            context.Response.Write(result);
            if (filters.Any())
            {
                ActionExcutedContent rc;
                foreach (var item in filters)
                {
                    rc = new ActionExcutedContent(paramslist,context,item.Message,result);
                    item.OnActionExcuted(rc);
                }
            }
        }

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
        private static object BuildResult(ServiceInfo info, object result)
        {
            var MethodAttrInfo = info.InvokeInfo.MethodAttrInfo;
            if ((MethodAttrInfo.MethodInfo.ReturnType.IsClass && MethodAttrInfo.MethodInfo.ReturnType!=typeof(string)) || MethodAttrInfo.MethodAttr.ResponseFormat == ResponseFormat.Json)
                result = new JavaScriptSerializer().Serialize(result);
            else if (MethodAttrInfo.MethodInfo.ReturnType.IsEnum)
                result = (int)result;
            return result;
        }
        private static IDictionary<string,object> StructureParams(HttpContext context, ServiceInfo info)
        {
            var destType = info.InvokeInfo.MethodAttrInfo.Parameters;
            var paramslist =new Dictionary<string,object>();
            for (var i = 0; i < destType.Length; i++)
            {
                Type type = destType[i].ParameterType;
                if (type == typeof(HttpRequest))
                {
                    paramslist.Add(destType[i].Name,context.Request);
                    continue;
                }
                else if (type == typeof(HttpContext))
                {
                    paramslist.Add(destType[i].Name, context);
                    continue;
                }
                var values = context.Request[destType[i].Name];
                if (values == null)
                    throw new ArgumentNullException(destType[i].Name);

                paramslist.Add(destType[i].Name, Convert.ChangeType(values, type));
            }
            return paramslist;
        }
        
    }
}
