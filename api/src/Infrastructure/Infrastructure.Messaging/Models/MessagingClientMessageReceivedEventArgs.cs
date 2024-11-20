namespace Homemap.Infrastructure.Messaging.Models
{
    internal class MessagingClientMessageReceivedEventArgs : EventArgs
    {
        public required string Topic { get; set; }

        public required byte[] Payload { get; set; }
    }
}
