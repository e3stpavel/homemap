using Homemap.Domain.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Homemap.Domain.Core
{
    public abstract class Device : AuditableEntity
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        public virtual Receiver Receiver { get; set; } = null!;

        // and after that you tell me that c# is great...
        public Type GetDeviceType()
        {
            Type deviceType = GetType();
            if (deviceType.Namespace == "Castle.Proxies" || deviceType.Namespace == "System.Data.Entity.DynamicProxies")
            {
                deviceType = deviceType.BaseType!;
            }

            return deviceType;
        }
    }
}
