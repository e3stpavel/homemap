namespace Homemap.ApplicationCore.Interfaces.Messaging
{
    public interface IMessagingService<T> : IDisposable
    {
        Task SubscribeAsync();

        Task UnsubscribeAsync();

        Task<T?> GetNextMessageAsync(CancellationToken cancellationToken = default);

        // TODO: think of API
        Task PublishAsync();
    }
}
