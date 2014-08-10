using FD.Service;
using FD.Service.Attribute;

namespace FdServiceTest.api
{
    public class logAttribute : FdFilterAttribute, IExceptionFilter
    {
        public override void OnActionAfter(ActionAfterContent rc)
        {
            var message = rc.Message;
        }

        public void OnException(ExceptionContent rc)
        {
            var message = rc.Message;
        }
    }
}