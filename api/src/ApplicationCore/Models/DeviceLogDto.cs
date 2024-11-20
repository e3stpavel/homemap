namespace Homemap.ApplicationCore.Models
{
    public record DeviceLogDto(string Level, string Message, DateTime Timestamp, int DeviceId);
}
