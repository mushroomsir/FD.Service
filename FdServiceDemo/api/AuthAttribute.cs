using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FD.Service;
namespace FdServiceTest.api
{
    public class AuthAttribute: FdFilterAttribute
    {

        public override void OnControllerBefore(ControllerBeforeContent rc)
        {
            var message = rc.Message;
        }
        public override void OnActionBefore(ActionBeforeContent rc)
        {
            var message = rc.Message;
        }
        public override void OnActionAfter(ActionAfterContent rc)
        {
            var message = rc.Message;
        }
    }
}