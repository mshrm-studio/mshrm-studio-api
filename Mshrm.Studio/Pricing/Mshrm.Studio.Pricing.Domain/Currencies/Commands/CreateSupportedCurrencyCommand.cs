using MediatR;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;

namespace Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Commands
{
    public class CreateSupportedCurrencyCommand : IRequest<Currency>
    {
        public string Name { get; private set; }
        public string Symbol { get; set; }
        public string SymbolNative { get; set; }
        public string? Description { get; set; }
        public PricingProviderType ProviderType { get; set; }
        public CurrencyType CurrencyType { get; set; }
        public Guid? LogoGuidId { get; set; }
    }
}
