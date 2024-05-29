namespace Mshrm.Studio.Pricing.Api.Models.Options
{
    /// <summary>
    /// Options for Mobula client
    /// </summary>
    public class MobulaServiceOptions
    {
        /// <summary>
        /// Mobula API endpoint
        /// </summary>
        public required string Endpoint { get; set; }

        /// <summary>
        /// Auth key
        /// </summary>
        public required string ApiKey { get; set; }
    }
}
