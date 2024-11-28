using Homemap.ApplicationCore.Models.DeviceStates.Core;

namespace Homemap.ApplicationCore.Models.DeviceStates
{
    public record LightbulbStateDto : DeviceStateDto
    {
        public int LightTemperature { get; init; }

        public int Brightness { get; init; }
    }
}
