using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Domain.ProviderAssets.Queries
{
    public class GetProviderAssetsQuery : IRequest<List<ProviderAsset>>
    {
        public PricingProviderType ProviderType { get; set; }
    }
}
