namespace Mshrm.Studio.Pricing.Api.Models.Options
{
    /// <summary>
    /// Options for FreeCurrency client
    /// </summary>
    public class FreeCurrencyOptions
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
