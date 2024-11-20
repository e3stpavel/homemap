using Homemap.ApplicationCore.Models;

namespace Homemap.ApplicationCore.Interfaces.Messaging
{
    public interface IDeviceLogMessagingService
    {
        /// <summary>
        /// Subscribes to specific topic to listen for device log messages within specific project
        /// </summary>
        Task ListenByProjectIdAsync(int projectId, CancellationToken cancellationToken);

        /// <summary>
        /// <list type="bullet">
        ///     <item>
        ///         <description>Waits for <c>SemaphoreSlim</c> to resolve</description>
        ///     </item>
        ///     <item>
        ///         <description>Dequeues the <c>DeviceLogDto</c> from <c>ConcurrentQueue</c></description>
        ///     </item>
        ///     <item>
        ///         <description>Validates <c>DeviceLogDto</c> using <c>IValidator</c></description>    
        ///     </item>
        /// </list>
        /// </summary>
        /// <returns><c>DeviceLogDto</c> if message successfully received and decoded, otherwise <c>null</c></returns>
        Task<DeviceLogMessageDto?> GetDeviceLogAsync(CancellationToken cancellationToken);
    }
}
