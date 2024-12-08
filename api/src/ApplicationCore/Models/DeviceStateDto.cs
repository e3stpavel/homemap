using Homemap.ApplicationCore.Constants;
using Homemap.ApplicationCore.Models.DeviceStates;
using System.Text.Json.Serialization;

namespace Homemap.ApplicationCore.Models
{
    [JsonDerivedType(typeof(ACStateDto), typeDiscriminator: DeviceConstants.AC)]
    [JsonDerivedType(typeof(ThermostatStateDto), typeDiscriminator: DeviceConstants.THERMOSTAT)]
    [JsonDerivedType(typeof(SocketStateDto), typeDiscriminator: DeviceConstants.SOCKET)]
    [JsonDerivedType(typeof(LightbulbStateDto), typeDiscriminator: DeviceConstants.LIGHTBULB)]
    public abstract record DeviceStateDto
    {
        public bool IsTurnedOn { get; init; }
    }
}
