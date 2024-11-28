using Homemap.Domain.Core;
using Homemap.Domain.Devices;

namespace Homemap.Domain.DeviceStates
{
    public class SocketState : DeviceState
    {
        public override bool IsAssignableTo(Device device)
        {
            if (device.GetDomainType() == typeof(SocketDevice))
                return true;

            return false;
        }
    }
}
