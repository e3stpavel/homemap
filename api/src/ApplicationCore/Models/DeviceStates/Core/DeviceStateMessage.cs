namespace Homemap.ApplicationCore.Models.DeviceStates.Core
{
    public record DeviceStateMessage
    {
        public required string Status { get; init; }

        public decimal? Temperature { get; set; }

        public int? LightTemperature { get; set; }

        public int? Brightness { get; set; }
    }
}
