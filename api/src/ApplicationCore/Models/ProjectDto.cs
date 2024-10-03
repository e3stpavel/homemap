﻿using Homemap.ApplicationCore.Models.Common;

namespace Homemap.ApplicationCore.Models
{
    public record ProjectDto(
        int Id, string Name, IReadOnlyCollection<ReceiverDto> Receivers) : EntityDto(Id);
}