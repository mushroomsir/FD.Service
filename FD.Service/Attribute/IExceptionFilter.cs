using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FD.Service.Attribute
{
    public interface IExceptionFilter
    {
        void OnException(ExceptionContent filterContext);
    }
}
