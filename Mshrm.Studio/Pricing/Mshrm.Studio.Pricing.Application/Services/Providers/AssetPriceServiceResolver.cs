using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Services.Providers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Pricing.Application.Services.Providers
{
    public delegate IAssetPriceProvider AssetPriceServiceResolver(PricingProviderType pricingProviderType);
}
