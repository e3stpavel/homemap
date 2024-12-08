namespace Homemap.ApplicationCore.Interfaces.Messaging
{
    public interface ISubscriptionService<T>
    {
        IAsyncEnumerable<T> StreamAsync(CancellationToken cancellationToken = default);
    }
}
