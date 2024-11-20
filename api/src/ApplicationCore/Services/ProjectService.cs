using AutoMapper;
using ErrorOr;
using Homemap.ApplicationCore.Errors;
using Homemap.ApplicationCore.Interfaces.Messaging;
using Homemap.ApplicationCore.Interfaces.Repositories;
using Homemap.ApplicationCore.Interfaces.Services;
using Homemap.ApplicationCore.Models;
using Homemap.Domain.Core;

namespace Homemap.ApplicationCore.Services
{
    public class ProjectService : BaseService<Project, ProjectDto>, IProjectService
    {
        private readonly IMapper _mapper;

        private readonly ICrudRepository<Project> _projectRepository;
        
        private readonly IDeviceRepository _deviceRepository;

        private readonly IDeviceLogMessagingService _deviceLogMessagingService;

        // TODO: consider better caching, cuz thread safety and memory considerations
        private IReadOnlyDictionary<int, Device>? _cachedProjectDevices;

        public ProjectService
        (
            IMapper mapper,
            ICrudRepository<Project> projectRepository,
            IDeviceRepository deviceRepository,
            IDeviceLogMessagingService deviceLogMessagingService
        ) : base(mapper, projectRepository)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
            _deviceRepository = deviceRepository;
            _deviceLogMessagingService = deviceLogMessagingService;
        }

        public async Task<ErrorOr<DeviceLogDto>> GetDeviceLogAsync(CancellationToken cancellationToken)
        {
            DeviceLogMessageDto? deviceLog = await _deviceLogMessagingService.GetDeviceLogAsync(cancellationToken);

            if (deviceLog is null)
                return ApplicationErrors.EmptyOrCorruptedMessage();

            if (_cachedProjectDevices is null)
                return ApplicationErrors.ValueNotPresent();

            if (!_cachedProjectDevices.TryGetValue(deviceLog.DeviceId, out Device? device))
                return ApplicationErrors.InappropriateMessage($"Unable to associate message with device ('{deviceLog.DeviceId}'). Device was not found.");

            // TODO: change later to DeviceLog entity and then map to dto
            DeviceDto deviceDto = _mapper.Map<DeviceDto>(device);

            return new DeviceLogDto
            {
                Level = deviceLog.Level,
                Message = deviceLog.Message,
                Timestamp = deviceLog.Timestamp,
                Device = deviceDto
            };
        }

        public async Task<ErrorOr<Success>> ListenDeviceLogsByIdAsync(int id, CancellationToken cancellationToken)
        {
            Project? project = await _projectRepository.FindByIdAsync(id);

            if (project is null)
                return UserErrors.EntityNotFound($"Project was not found ('{id}')");

            _cachedProjectDevices = await _deviceRepository.FindAllByProjectId(project.Id);

            await _deviceLogMessagingService.ListenByProjectIdAsync(project.Id, cancellationToken);

            return Result.Success;
        }
    }
}
