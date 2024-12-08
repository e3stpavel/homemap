namespace Homemap.ApplicationCore.Models.DeviceStates
{
    public record ThermostatStateDto : DeviceStateDto
    {
        public decimal Temperature { get; init; }
    }
}
