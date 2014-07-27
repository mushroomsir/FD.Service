using System;
using System.Collections.Generic;
using System.Web;


namespace FD.Service
{
    internal class FiltersInvoker
    {

        internal static void OnControllerBefore(HttpContext context, IEnumerable<FdFilterAttribute> filters)
        {
            foreach (var item in filters)
            {
                var rc = new ControllerBeforeContent(context, item.Message);
                item.OnControllerBefore(rc);
            }
        }

        internal static void OnActionBefore(HttpContext context, IEnumerable<FdFilterAttribute> filters,
            IDictionary<string, object> paramslist)
        {
            foreach (var item in filters)
            {
                var rc = new ActionBeforeContent(paramslist, context, item.Message);
                item.OnActionBefore(rc);
            }
        }

        internal static void OnActionAfter(HttpContext context, IEnumerable<FdFilterAttribute> filters,
            IDictionary<string, object> paramslist, object result)
        {
            foreach (var item in filters)
            {
                var rc = new ActionAfterContent(paramslist, context, item.Message, result);
                item.OnActionAfter(rc);
            }
        }

        internal static void OnResultBefore(HttpContext context, IEnumerable<FdFilterAttribute> filters,
            IDictionary<string, object> paramslist, object result)
        {
            foreach (var item in filters)
            {
                var rc = new ResultBeforeContent(paramslist, context, item.Message, result);
                item.OnActionBefore(rc);
            }
        }

        internal static void OnResultAfter(HttpContext context, IEnumerable<FdFilterAttribute> filters,
            IDictionary<string, object> paramslist, object result)
        {
            foreach (var item in filters)
            {
                var rc = new ResultAfterContent(paramslist, context, item.Message, result);
                item.OnResultAfter(rc);
            }
        }

        internal static void OnActionException(HttpContext context, IEnumerable<FdFilterAttribute> filters,
            IDictionary<string, object> paramslist, Exception ex)
        {
            foreach (var item in filters)
            {
                var rc = new ActionExceptionContent(paramslist, context, item.Message, ex);
                item.OnActionException(rc);
            }
        }

    }
}
