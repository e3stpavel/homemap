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
        private readonly ICrudRepository<Project> _repository;

        private readonly IDeviceLogMessagingService _deviceLogMessagingService;

        public ProjectService
        (
            IMapper mapper,
            ICrudRepository<Project> repository,
            IDeviceLogMessagingService deviceLogMessagingService
        ) : base(mapper, repository)
        {
            _repository = repository;
            _deviceLogMessagingService = deviceLogMessagingService;
        }

        public async Task<ErrorOr<DeviceLogDto>> GetDeviceLogAsync(CancellationToken cancellationToken)
        {
            // TODO: query all project devices and store them somewhere
            //  then check if DeviceLog deviceId is really in project
            //  if not reject such log as obsolete
            DeviceLogDto? deviceLog = await _deviceLogMessagingService.GetDeviceLogAsync(cancellationToken);

            if (deviceLog is null)
                return ApplicationErrors.EmptyOrCorruptedMessage();

            return deviceLog;
        }

        public async Task<ErrorOr<Success>> ListenDeviceLogsByIdAsync(int id, CancellationToken cancellationToken)
        {
            Project? project = await _repository.FindByIdAsync(id);

            if (project is null)
                return UserErrors.EntityNotFound($"Project was not found ('{id}')");

            await _deviceLogMessagingService.ListenByProjectIdAsync(project.Id, cancellationToken);

            return Result.Success;
        }
    }
}
