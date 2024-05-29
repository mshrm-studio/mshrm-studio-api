namespace Mshrm.Studio.Pricing.Api.Models.Options
{
    /// <summary>
    /// Options for Twleve data client
    /// </summary>
    public class TwelveDataServiceOptions
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
