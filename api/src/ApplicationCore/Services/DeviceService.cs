using AutoMapper;
using ErrorOr;
using Homemap.ApplicationCore.Errors;
using Homemap.ApplicationCore.Interfaces.Messaging;
using Homemap.ApplicationCore.Interfaces.Repositories;
using Homemap.ApplicationCore.Interfaces.Services;
using Homemap.ApplicationCore.Models;
using Homemap.ApplicationCore.Models.DeviceStates.Core;
using Homemap.Domain.Core;
using Homemap.Domain.Devices;
using Homemap.Domain.DeviceStates;

namespace Homemap.ApplicationCore.Services
{
    public class DeviceService : BaseService<Device, DeviceDto>, IDeviceService
    {
        private readonly IMapper _mapper;

        private readonly IDeviceRepository _deviceRepository;

        private readonly ICrudRepository<Receiver> _receiverRepository;

        private readonly IMessagingServiceFactory _messagingServiceFactory;

        public DeviceService(
            IMapper mapper,
            IDeviceRepository deviceRepository,
            ICrudRepository<Receiver> receiverRepository,
            IMessagingServiceFactory messagingServiceFactory
        ) : base(mapper, deviceRepository)
        {
            _deviceRepository = deviceRepository;
            _receiverRepository = receiverRepository;
            _mapper = mapper;
            _messagingServiceFactory = messagingServiceFactory;
        }

        public async Task<ErrorOr<IReadOnlyList<DeviceDto>>> GetAllAsync(int receiverId)
        {
            Receiver? receiver = await _receiverRepository.FindByIdAsync(receiverId);

            if (receiver == null)
            {
                return UserErrors.EntityNotFound($"Receiver was not found ('{receiverId}')");
            }

            IReadOnlyList<Device> devices = await _deviceRepository.FindAllByReceiverIdAsync(receiverId);
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

        public async Task<ErrorOr<DeviceStateDto>> GetDeviceStateByIdAsync(int id, CancellationToken cancellationToken)
        {
            Device? device = await _deviceRepository.FindByIdAsync(id);

            if (device is null)
                return UserErrors.EntityNotFound($"Device was not found ('{id}')");

            // assume that device has published the initial state message with retain flag
            DeviceStateMessage? stateMessage;
            string topic = $"prj/{device.Receiver.ProjectId}/rcv/{device.ReceiverId}/dev/{id}/state";

            using (var subscriptionService = _messagingServiceFactory.CreateSubscriptionService<DeviceStateMessage>(topic))
            {
                await subscriptionService.SubscribeAsync();
                stateMessage = await subscriptionService.GetNextMessageAsync(cancellationToken);
                await subscriptionService.UnsubscribeAsync();
            }

            if (stateMessage is null)
                return ApplicationErrors.EmptyOrCorruptedMessage();

            // TODO: this is not scallable and can be prettier, but I am tired of stupid c# types behaviour
            DeviceState? state = device.GetDeviceType() switch
            {
                Type type when type == typeof(ACDevice) => _mapper.Map<ACState>(stateMessage),
                Type type when type == typeof(ThermostatDevice) => _mapper.Map<ThermostatState>(stateMessage),
                Type type when type == typeof(SocketDevice) => _mapper.Map<SocketState>(stateMessage),
                Type type when type == typeof(LightbulbDevice) => _mapper.Map<LightbulbState>(stateMessage),
                _ => default
            };

            if (state is null)
                return ApplicationErrors.NotImplemented();

            return _mapper.Map<DeviceStateDto>(state);
        }

        public async Task<ErrorOr<Updated>> SetDeviceStateByIdAsync(int id, DeviceStateDto stateDto)
        {
            Device? device = await _deviceRepository.FindByIdAsync(id);

            if (device is null)
                return UserErrors.EntityNotFound($"Device was not found ('{id}')");

            DeviceState state = _mapper.Map<DeviceState>(stateDto);
            if (!state.IsAssignableTo(device))
                return UserErrors.IllegalOperation("Device state is not assignable to this device");

            string topic = $"prj/{device.Receiver.ProjectId}/rcv/{device.ReceiverId}/dev/{id}/set-state";
            var publishingService = _messagingServiceFactory.CreatePublishingService<DeviceStateMessage>(topic);

            DeviceStateMessage stateMessage = _mapper.Map<DeviceStateMessage>(state);
            await publishingService.PublishAsync(stateMessage);

            return Result.Updated;
        }
    }
}
