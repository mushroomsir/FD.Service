using System;
using System.Collections.Generic;
using System.Reflection;
using FD.Service.Attribute;

namespace FD.Service.Model
{
    internal class MethodAndAttrInfo
    {
        public MethodInfo MethodInfo { get; set; }
        public FdMethodAttribute MethodAttr { get; set; }
        public FdFilterAttribute[] FiltersAttr { get; set; }
        public ParameterInfo[] Parameters { get; set; }
        public IEnumerable<IExceptionFilter> ExceptionFilters { get; set; }
    }
}
