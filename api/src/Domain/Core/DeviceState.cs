namespace Homemap.Domain.Core
{
    public abstract class DeviceState
    {
        public bool IsTurnedOn { get; set; }

        public abstract bool IsAssignableTo(Device device);
    }
}
