namespace Homemap.ApplicationCore.Models
{
    public record DeviceLogMessageDto(string Level, string Message, DateTime Timestamp, int DeviceId);
}
