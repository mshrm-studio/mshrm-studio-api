﻿using Mshrm.Studio.Api.Clients.Pricing;
using Newtonsoft.Json;

namespace Mshrm.Studio.Api.Models.Dtos.Currencies
{
    public class CurrencyResponseDto
    {
        [JsonProperty("guidId")]
        public Guid GuidId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("symbolNative")]
        public string SymbolNative { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("providerType")]
        public PricingProviderType ProviderType { get; set; }

        [JsonProperty("currencyType")]
        public CurrencyType CurrencyType { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("LogoUrl")]
        public string? LogoUrl { get; set; }

        /// <summary>
        /// Logo GUID
        /// </summary>
        [JsonProperty("logoGuidId")]
        public Guid? logoGuidId { get; set; }
    }
}
