
namespace FD.Service.Model
{
    internal class InvokeInfo
    {
        public TypeAndAttrInfo ServiceTypeInfo { get; set; }
        public MethodAndAttrInfo MethodAttrInfo { get; set; }
        public object ServiceInstance { get; set; }
    }
}
