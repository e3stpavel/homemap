namespace Homemap.ApplicationCore.Models
{
    public record DeviceLogDto
    {
        public required string Level { get; init; }

        public required string Message { get; init; }

        public DateTime Timestamp { get; init; }

        public required DeviceDto Device { get; init; }
    }
}
