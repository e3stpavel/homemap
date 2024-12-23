﻿using AutoMapper;
using ErrorOr;
using Homemap.ApplicationCore.Errors;
using Homemap.ApplicationCore.Interfaces.Messaging;
using Homemap.ApplicationCore.Interfaces.Repositories;
using Homemap.ApplicationCore.Interfaces.Services;
using Homemap.ApplicationCore.Models;
using Homemap.ApplicationCore.Models.Messaging;
using Homemap.Domain.Core;
using System.Runtime.CompilerServices;

namespace Homemap.ApplicationCore.Services
{
    public class ProjectService : BaseService<Project, ProjectDto>, IProjectService
    {
        private readonly IMapper _mapper;

        private readonly ICrudRepository<Project> _projectRepository;

        private readonly IDeviceRepository _deviceRepository;

        private readonly IMessagingServiceFactory _messagingServiceFactory;

        public ProjectService
        (
            IMapper mapper,
            ICrudRepository<Project> projectRepository,
            IDeviceRepository deviceRepository,
            IMessagingServiceFactory messagingServiceFactory
        ) : base(mapper, projectRepository)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
            _deviceRepository = deviceRepository;
            _messagingServiceFactory = messagingServiceFactory;
        }

        public async IAsyncEnumerable<ErrorOr<DeviceLogDto>> GetDeviceLogsByIdAsync(int id, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            Project? project = await _projectRepository.FindByIdAsync(id);

            if (project is null)
            {
                yield return UserErrors.EntityNotFound($"Project was not found ('{id}')");
                yield break;
            }

            IReadOnlyDictionary<int, DeviceDto> projectDeviceDtos = _mapper.Map<IReadOnlyDictionary<int, DeviceDto>>(
                await _deviceRepository.FindAllByProjectIdAsync(id)
            );

            string topic = $"prj/{id}/rcv/+/dev/+/logs";
            var subscriptionService = _messagingServiceFactory.CreateSubscriptionService<LogMessageDto>(topic);
            var stream = subscriptionService.StreamAsync(cancellationToken);

            await foreach (var logMessage in stream)
            {
                if (!projectDeviceDtos.TryGetValue(logMessage.DeviceId, out var deviceDto))
                    continue;

                yield return new DeviceLogDto
                {
                    Level = logMessage.Level,
                    Message = logMessage.Message,
                    Timestamp = logMessage.Timestamp,
                    Device = deviceDto
                };
            }
        }
    }
}
