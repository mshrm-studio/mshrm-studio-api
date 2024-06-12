using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Cache;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Domain.AssetPrices.Queries
{
    public class GetProviderPricesQuery : IRequest<List<PricePair>>
    {
        public PricingProviderType ProviderType { get; set; }
        public List<string> ProviderCurrencySymbols { get; set; }
    }
}
