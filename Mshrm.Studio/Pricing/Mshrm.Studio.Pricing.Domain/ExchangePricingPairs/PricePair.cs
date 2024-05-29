namespace Mshrm.Studio.Pricing.Api.Models.Cache
{
    public class PricePair
    {
        public string BaseCurrency { get; set; }
        public string Currency { get; set; }
        public decimal Price { get; set; }
        public decimal? Volume { get; set; }
        public decimal? MarketCap { get; set; }
    }
}
