namespace Homemap.ApplicationCore.Models.DeviceLogs
{
    public record DeviceLogDto : AbstractDeviceLogDto
    {
        public required DeviceDto Device { get; init; }
    }
}
