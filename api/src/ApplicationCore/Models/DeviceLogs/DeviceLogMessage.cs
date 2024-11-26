namespace Homemap.ApplicationCore.Models.DeviceLogs
{
    public record DeviceLogMessage : AbstractDeviceLogDto
    {
        public int DeviceId { get; init; }
    }
}
