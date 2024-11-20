using ErrorOr;
using Homemap.ApplicationCore.Models;

namespace Homemap.ApplicationCore.Interfaces.Services
{
    public interface IProjectService : IService<ProjectDto>
    {
        Task<ErrorOr<Success>> ListenDeviceLogsByIdAsync(int id, CancellationToken cancellationToken);

        Task<ErrorOr<DeviceLogDto>> GetDeviceLogAsync(CancellationToken cancellationToken);
    }
}
