using ErrorOr;
using Homemap.ApplicationCore.Models;

namespace Homemap.ApplicationCore.Interfaces.Services
{
    public interface IDeviceService : IService<DeviceDto>
    {
        Task<ErrorOr<IReadOnlyList<DeviceDto>>> GetAllAsync(int receiverId);

        Task<ErrorOr<DeviceDto>> CreateAsync(int receiverId, DeviceDto dto);

        Task<ErrorOr<DeviceStateDto>> GetDeviceStateByIdAsync(int id, CancellationToken cancellationToken);

        Task<ErrorOr<Updated>> SetDeviceStateByIdAsync(int id, DeviceStateDto state);
    }
}
