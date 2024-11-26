using ErrorOr;
using Homemap.ApplicationCore.Models;
using Homemap.ApplicationCore.Models.DeviceLogs;

namespace Homemap.ApplicationCore.Interfaces.Services
{
    public interface IProjectService : IService<ProjectDto>
    {
        IAsyncEnumerable<ErrorOr<DeviceLogDto>> GetDeviceLogsByIdAsync(int id, CancellationToken cancellationToken);
    }
}
