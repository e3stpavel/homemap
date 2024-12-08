namespace Homemap.ApplicationCore.Models.Messaging
{
    public record LogMessageDto
    {
        public required string Level { get; init; }

        public required string Message { get; init; }

        public DateTime Timestamp { get; init; }

        public int DeviceId { get; init; }
    }
}
