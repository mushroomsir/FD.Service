using FD.Service;

namespace FdServiceTest.api
{
    public class logAttribute : FdFilterAttribute
    {
        public override void OnActionAfter(ActionAfterContent rc)
        {
            var message = rc.Message;
        }
        public override void OnResultAfter(ResultAfterContent rc)
        {
            var message = rc.Message;
        }
        public override void OnActionException(ActionExceptionContent rc)
        {
            var message = rc.Message;
        }
    }
}