
namespace FD.Service.Model
{

    internal class ServiceInfo
    {
        internal ServiceInfo(NamesPair pair, InvokeInfo vkInfo)
        {
            this.NamesPair = pair;
            this.InvokeInfo = vkInfo;
        }

        public NamesPair NamesPair { get; private set; }
        public InvokeInfo InvokeInfo { get; private set; }

    }
}
