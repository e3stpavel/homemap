namespace Homemap.ApplicationCore.Interfaces.Messaging
{
    public interface IMessagingServiceFactory
    {
        ISubscriptionService<T> CreateSubscriptionService<T>(string topic);

        IPublishingService<T> CreatePublishingService<T>(string topic);
    }
}
