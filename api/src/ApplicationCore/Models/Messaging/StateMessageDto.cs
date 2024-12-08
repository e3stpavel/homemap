namespace Homemap.ApplicationCore.Models.Messaging
{
    public record StateMessageDto
    {
        public required string Status { get; init; }

        public decimal? Temperature { get; set; }

        public int? LightTemperature { get; set; }

        public int? Brightness { get; set; }
    }
}
