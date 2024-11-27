namespace Homemap.ApplicationCore.Interfaces.Messaging
{
    public interface IPublishingService<T>
    {
        Task PublishAsync(T message);
    }
}
