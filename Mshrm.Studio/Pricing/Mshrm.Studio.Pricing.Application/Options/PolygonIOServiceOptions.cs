namespace Mshrm.Studio.Pricing.Api.Models.Options
{
    /// <summary>
    /// Options for PolygonIO client
    /// </summary>
    public class PolygonIOServiceOptions
    {
        /// <summary>
        /// Api endpoint
        /// </summary>
        public required string Endpoint { get; set; }

        /// <summary>
        /// Auth key
        /// </summary>
        public required string ApiKey { get; set; }

        /// <summary>
        /// How many requests are allowed per minute
        /// </summary>
        public decimal RequestsPerMinute { get; set; }
    }
}
