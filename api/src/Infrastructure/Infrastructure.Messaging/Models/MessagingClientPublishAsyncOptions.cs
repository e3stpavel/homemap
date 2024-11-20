namespace Homemap.Infrastructure.Messaging.Models
{
    internal record MessagingClientPublishAsyncOptions
    {
        public enum QualityOfService
        {
            AT_MOST_ONCE = 0x00,
            AT_LEAST_ONCE = 0x01,
            EXACTLY_ONCE = 0x02
        }

        public QualityOfService QoS { get; init; } = QualityOfService.AT_MOST_ONCE;

        public string ContentType { get; init; } = "application/json";

        public bool IsRetain { get; init; } = false;

    }
}
