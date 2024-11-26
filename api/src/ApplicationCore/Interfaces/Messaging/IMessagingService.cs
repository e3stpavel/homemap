namespace Homemap.ApplicationCore.Interfaces.Messaging
{
    public interface IMessagingService<T> : IDisposable
    {
        Task SubscribeAsync();

        Task UnsubscribeAsync();

        Task<T?> GetNextMessage(CancellationToken cancellationToken = default);

        // TODO: think of API
        Task PublishAsync();
    }
}
