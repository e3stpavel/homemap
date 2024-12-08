using AutoMapper;
using Homemap.ApplicationCore.Models;
using Homemap.ApplicationCore.Models.DeviceStates;
using Homemap.ApplicationCore.Models.Messaging;
using Homemap.Domain.Core;
using Homemap.Domain.DeviceStates;

namespace Homemap.ApplicationCore.Mappings
{
    internal class DeviceStateProfile : Profile
    {
        public DeviceStateProfile()
        {
            CreateMap<DeviceState, DeviceStateDto>()
                .IncludeAllDerived()
                .ReverseMap()
                .IncludeAllDerived();

            CreateMap<ACState, ACStateDto>()
                .ReverseMap();

            CreateMap<LightbulbState, LightbulbStateDto>()
                .ReverseMap();

            CreateMap<SocketState, SocketStateDto>()
                .ReverseMap();

            CreateMap<ThermostatState, ThermostatStateDto>()
                .ReverseMap();

            CreateMap<DeviceState, StateMessageDto>()
                .ForMember(nameof(StateMessageDto.Status),
                    opt => opt.MapFrom(src => src.IsTurnedOn ? "on" : "off"))
                .IncludeAllDerived()
                .ReverseMap()
                .ForMember(nameof(DeviceState.IsTurnedOn),
                    opt => opt.MapFrom(src => src.Status == "on"))
                .IncludeAllDerived();

            CreateMap<ACState, StateMessageDto>()
                .ReverseMap();

            CreateMap<LightbulbState, StateMessageDto>()
                .ReverseMap();

            CreateMap<SocketState, StateMessageDto>()
                .ReverseMap();

            CreateMap<ThermostatState, StateMessageDto>()
                .ReverseMap();
        }
    }
}
