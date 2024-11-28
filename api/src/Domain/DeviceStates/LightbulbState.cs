using Homemap.Domain.Core;
using Homemap.Domain.Devices;

namespace Homemap.Domain.DeviceStates
{
    public class LightbulbState : DeviceState
    {
        public int LightTemperature { get; set; }

        public int Brightness { get; set; }

        public override bool IsAssignableTo(Device device)
        {
            if (device.GetDomainType() == typeof(LightbulbDevice))
                return true;

            return false;
        }
    }
}
