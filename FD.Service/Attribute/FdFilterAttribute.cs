using System;

namespace FD.Service
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public abstract class FdFilterAttribute : System.Attribute
    {
        public int Order { get; set; }
        public string Message { get; set; }

        public virtual void OnControllerBefore(ControllerBeforeContent rc) 
        {
        }
        public virtual void OnActionBefore(ActionBeforeContent rc) 
        {
        }
        public virtual void OnActionAfter(ActionAfterContent rc)
        {
        }
    }
}
