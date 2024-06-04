namespace Mshrm.Studio.Pricing.Api.Models.Provider
{
    /// <summary>
    /// Exchange rate
    /// </summary>
    public class ExchangeRate
    {
        /// <summary>
        /// From asset
        /// </summary>
        public required string From { get; set; }

        /// <summary>
        /// To asset
        /// </summary>
        public required string To { get; set; }

        /// <summary>
        /// When the rate has been generated for
        /// </summary>
        public DateTime TimeStamp { get; set; }
    }
}
