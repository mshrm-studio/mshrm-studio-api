namespace Mshrm.Studio.Pricing.Api.Models.Options
{
    /// <summary>
    /// Options for MetalsDev client
    /// </summary>
    public class MetalsDevServiceOptions
    {
        /// <summary>
        /// Api endpoint
        /// </summary>
        public required string Endpoint { get; set; }

        /// <summary>
        /// Auth key
        /// </summary>
        public required string ApiKey { get; set; }
    }
}
