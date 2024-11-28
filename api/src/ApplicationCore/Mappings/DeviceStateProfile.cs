using AutoMapper;
using Homemap.ApplicationCore.Models.DeviceStates;
using Homemap.ApplicationCore.Models.DeviceStates.Core;
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

            CreateMap<DeviceState, DeviceStateMessage>()
                .ForMember(nameof(DeviceStateMessage.Status),
                    opt => opt.MapFrom(src => src.IsTurnedOn ? "on" : "off"))
                .IncludeAllDerived()
                .ReverseMap()
                .ForMember(nameof(DeviceState.IsTurnedOn),
                    opt => opt.MapFrom(src => src.Status == "on"))
                .IncludeAllDerived();

            CreateMap<ACState, DeviceStateMessage>()
                .ReverseMap();

            CreateMap<LightbulbState, DeviceStateMessage>()
                .ReverseMap();

            CreateMap<SocketState, DeviceStateMessage>()
                .ReverseMap();

            CreateMap<ThermostatState, DeviceStateMessage>()
                .ReverseMap();
        }
    }
}
