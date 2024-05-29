using MediatR;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.ExchangePricingPairs.Commands;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers;
using Mshrm.Studio.Pricing.Application.Services.Providers;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using OpenTracing;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.Currencies
{
    public class CreateSupportedCurrencyCommandHandler : IRequestHandler<CreateSupportedCurrencyCommand, Currency>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ITracer _tracer;
        private readonly CurrencyPriceServiceResolver _currencyPriceServiceResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSupportedCurrencyCommandHandler"/> class.
        /// </summary>
        /// <param name="currencyRepository"></param>
        /// <param name="tracer"></param>
        public CreateSupportedCurrencyCommandHandler(CurrencyPriceServiceResolver currencyPriceServiceResolver, ICurrencyRepository currencyRepository, ITracer tracer)
        {
            _currencyRepository = currencyRepository;
            _currencyPriceServiceResolver = currencyPriceServiceResolver;

            _tracer = tracer;
        }

        /// <summary>
        /// Create a new currency
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The new currency</returns>
        public async Task<Currency> Handle(CreateSupportedCurrencyCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateCurrencyAsync_CreateCurrencyService").StartActive(true))
            {
                // Check symbol already exists for currency type
                var existingCurrency = await _currencyRepository.GetCurrencyAsync(command.Symbol, command.CurrencyType, true, cancellationToken);
                if (existingCurrency != null)
                    throw new UnprocessableEntityException("The currency already exists", FailureCode.CurrencyAlreadyExists);

                // Check provider can support currency import
                var provider = _currencyPriceServiceResolver(command.ProviderType);
                var isSupportedByProvider = await provider.IsCurrencySupportedAsync(command.Symbol);
                if (!isSupportedByProvider)
                    throw new UnprocessableEntityException("Currency is not supported by the provider", FailureCode.CurrencyNotSupported);

                // Create
                return await _currencyRepository.CreateCurrencyAsync(command.Name, command.Description, command.ProviderType, command.CurrencyType, command.Symbol,
                    command.SymbolNative, command.LogoGuidId, cancellationToken);
            }
        }
    }
}
