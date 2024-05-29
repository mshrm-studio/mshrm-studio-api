using MediatR;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Commands;
using Mshrm.Studio.Pricing.Api.Models.CQRS.Currencies.Queries;
using Mshrm.Studio.Pricing.Api.Models.Entites;
using Mshrm.Studio.Pricing.Api.Models.Enums;
using Mshrm.Studio.Pricing.Api.Repositories;
using Mshrm.Studio.Pricing.Api.Repositories.Interfaces;
using Mshrm.Studio.Pricing.Api.Services.Providers;
using Mshrm.Studio.Pricing.Application.Services.Providers;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions;
using Mshrm.Studio.Shared.Exceptions.HttpAction;
using Mshrm.Studio.Shared.Models.Pagination;
using OpenTracing;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mshrm.Studio.Pricing.Application.Handlers.Request.Currencies
{
    public class UpdateSupportedCurrencyCommandHandler : IRequestHandler<UpdateSupportedCurrencyCommand, Currency>
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly CurrencyPriceServiceResolver _currencyPriceServiceResolver;
        private readonly ITracer _tracer;

        public UpdateSupportedCurrencyCommandHandler(ICurrencyRepository currencyRepository, CurrencyPriceServiceResolver currencyPriceServiceResolver, ITracer tracer)
        {
            _currencyRepository = currencyRepository;

            _tracer = tracer;
            _currencyPriceServiceResolver = currencyPriceServiceResolver;
        }

        /// <summary>
        /// Update a currency (all except symbol)
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken">A stopping token</param>
        /// <returns>The updated currency</returns>
        public async Task<Currency> Handle(UpdateSupportedCurrencyCommand command, CancellationToken cancellationToken)
        {
            using (var scope = _tracer.BuildSpan("CreateCurrencyAsync_CreateCurrencyService").StartActive(true))
            {
                // Check symbol already exists for currency type
                var existingCurrency = await _currencyRepository.GetCurrencyAsync(command.CurrencyId, true, cancellationToken);
                if (existingCurrency == null)
                    throw new NotFoundException("The currency doesn't exists", FailureCode.CurrencyDoesntExist);

                // Check provider can support currency import
                var provider = _currencyPriceServiceResolver(command.ProviderType);
                var isSupportedByProvider = await provider.IsCurrencySupportedAsync(existingCurrency.Symbol);
                if (!isSupportedByProvider)
                    throw new UnprocessableEntityException("Currency is not supported by the provider", FailureCode.CurrencyNotSupported);

                // Update
                return await _currencyRepository.UpdateCurrencyAsync(command.CurrencyId, command.Name, command.Description, command.ProviderType,
                    command.CurrencyType, command.SymbolNative, command.LogoGuidId, cancellationToken);
            }
        }
    }
}
