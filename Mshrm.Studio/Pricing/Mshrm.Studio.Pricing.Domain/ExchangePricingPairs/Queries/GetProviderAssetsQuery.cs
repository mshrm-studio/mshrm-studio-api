using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Domain.ExchangePricingPairs.Queries
{
    public class GetProviderAssetsQuery : IRequest<List<string>>
    {
        public PricingProviderType ProviderType { get; set; }
    }
}
