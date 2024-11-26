namespace Homemap.ApplicationCore.Models
{
    public abstract record AbstractDeviceLogDto
    {
        public required string Level { get; init; }

        public required string Message { get; init; }

        public DateTime Timestamp { get; init; }
    }
}
