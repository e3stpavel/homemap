namespace Homemap.Infrastructure.Messaging.Models
{
    internal record MessagingClientOptions
    {
        public required string ConnectionUri { get; init; }
    }
}
