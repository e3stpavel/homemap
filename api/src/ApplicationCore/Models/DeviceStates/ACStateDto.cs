using Homemap.ApplicationCore.Models.DeviceStates.Core;

namespace Homemap.ApplicationCore.Models.DeviceStates
{
    public record ACStateDto : DeviceStateDto
    {
        public decimal Temperature { get; init; }
    }
}
