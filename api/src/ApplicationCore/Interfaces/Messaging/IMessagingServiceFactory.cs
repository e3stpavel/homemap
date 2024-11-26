namespace Homemap.ApplicationCore.Interfaces.Messaging
{
    public interface IMessagingServiceFactory
    {
        IMessagingService<T> Create<T>(string topic);
    }
}
