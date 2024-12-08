using Homemap.Domain.Core;
using Homemap.Domain.Devices;

namespace Homemap.Domain.DeviceStates
{
    public class ACState : DeviceState
    {
        public decimal Temperature { get; set; }

        public override bool IsAssignableTo(Device device)
        {
            if (device.GetDeviceType() == typeof(ACDevice))
                return true;

            return false;
        }
    }
}
