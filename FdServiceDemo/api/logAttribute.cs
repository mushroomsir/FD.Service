using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FD.Service;
namespace FdServiceTest.api
{
    public class logAttribute : FdFilterAttribute
    {
       
        public override void OnActionAfter(ActionAfterContent rc)
        {
            var message = rc.Message;
        }
        public override void OnActionExcuted(ActionExcutedContent rc)
        {
            var message = rc.Message;
        }
        public override void OnExceptionExecuting(ActionExceptionContent rc)
        {
            var message = rc.Message;
        }
    }
}