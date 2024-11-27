using Homemap.Domain.Core;
namespace Homemap.ApplicationCore.Interfaces.Repositories;

public interface IDeviceRepository : ICrudRepository<Device>
{
    Task<IReadOnlyList<Device>> FindAllByReceiverIdAsync(int receiverId);

    Task<IReadOnlyDictionary<int, Device>> FindAllByProjectIdAsync(int projectId);
}
