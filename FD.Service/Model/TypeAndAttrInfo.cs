﻿using System;

namespace FD.Service.Model
{
    internal class TypeAndAttrInfo
    {
        public Type ServiceType { get; set; }
        public FdServiceAttribute ServiceAttr { get; set; }
        public FdFilterAttribute[] FiltersAttr { get; set; }
    }
	
}
