namespace Homemap.ApplicationCore.Interfaces.Messaging
{
    public interface ISubscriptionService<T> : IDisposable
    {
        Task SubscribeAsync();

        Task UnsubscribeAsync();

        Task<T?> GetNextMessageAsync(CancellationToken cancellationToken = default);
    }
}
