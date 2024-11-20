using AutoMapper;
using ErrorOr;
using Homemap.ApplicationCore.Errors;
using Homemap.ApplicationCore.Interfaces.Repositories;
using Homemap.ApplicationCore.Interfaces.Services;
using Homemap.ApplicationCore.Models;
using Homemap.Domain.Core;

namespace Homemap.ApplicationCore.Services
{
    public class DeviceService : BaseService<Device, DeviceDto>, IDeviceService
    {
        private readonly IMapper _mapper;

        private readonly IDeviceRepository _deviceRepository;

        private readonly ICrudRepository<Receiver> _receiverRepository;

        public DeviceService(
            IMapper mapper,
            IDeviceRepository deviceRepository,
            ICrudRepository<Receiver> receiverRepository
        ) : base(mapper, deviceRepository)
        {
            _deviceRepository = deviceRepository;
            _receiverRepository = receiverRepository;
            _mapper = mapper;
        }

        public async Task<ErrorOr<IReadOnlyList<DeviceDto>>> GetAllAsync(int receiverId)
        {
            Receiver? receiver = await _receiverRepository.FindByIdAsync(receiverId);

            if (receiver == null)
            {
                return UserErrors.EntityNotFound($"Receiver was not found ('{receiverId}')");
            }

            IReadOnlyList<Device> devices = await _deviceRepository.FindAllByReceiverId(receiverId);
            return _mapper.Map<IReadOnlyList<DeviceDto>>(devices).ToErrorOr();
        }

        public async Task<ErrorOr<DeviceDto>> CreateAsync(int receiverId, DeviceDto dto)
        {
            Receiver? receiver = await _receiverRepository.FindByIdAsync(receiverId);

            if (receiver == null)
            {
                return UserErrors.EntityNotFound($"Receiver was not found ('{receiverId}')");
            }

            Device deviceEntity = _mapper.Map<Device>(dto);
            deviceEntity.Receiver = receiver;

            await _deviceRepository.AddAsync(deviceEntity);
            await _deviceRepository.SaveAsync();

            return _mapper.Map<DeviceDto>(deviceEntity);
        }
    }
}
