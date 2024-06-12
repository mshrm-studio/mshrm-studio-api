using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;

namespace Mshrm.Studio.Api.Models.Options.Configurations
{
    public class ConfigureModelBindingLocalization : IConfigureOptions<MvcOptions>
    {
        private readonly IServiceScopeFactory _serviceFactory;
        public ConfigureModelBindingLocalization(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public void Configure(MvcOptions options)
        {
            using (var scope = _serviceFactory.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var localizer = provider.GetRequiredService<IStringLocalizer>();

                options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((x, y) =>
                    localizer[nameof(options.ModelBindingMessageProvider.AttemptedValueIsInvalidAccessor), x, y]); //AttemptedValueIsInvalidAccessor "The value '{0}' is not valid for {1}."

                options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor((x) =>
                    localizer[nameof(options.ModelBindingMessageProvider.MissingBindRequiredValueAccessor), x]);//MissingBindRequiredValueAccessor "A value for the '{0}' parameter or property was not provided."

                options.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() =>
                    localizer[nameof(options.ModelBindingMessageProvider.MissingKeyOrValueAccessor)]);//MissingKeyOrValueAccessor "A value is required."

                options.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() =>
                    localizer[nameof(options.ModelBindingMessageProvider.MissingRequestBodyRequiredValueAccessor)]);//MissingRequestBodyRequiredValueAccessor "A non-empty request body is required."

                options.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor((x) =>
                    localizer[nameof(options.ModelBindingMessageProvider.NonPropertyAttemptedValueIsInvalidAccessor), x]);//NonPropertyAttemptedValueIsInvalidAccessor "The value '{0}' is not valid."

                options.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() =>
                    localizer[nameof(options.ModelBindingMessageProvider.NonPropertyUnknownValueIsInvalidAccessor)]);//NonPropertyUnknownValueIsInvalidAccessor "The supplied value is invalid."

                options.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() =>
                    localizer[nameof(options.ModelBindingMessageProvider.NonPropertyValueMustBeANumberAccessor)]);//NonPropertyValueMustBeANumberAccessor "The field must be a number."

                options.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor((x) =>
                    localizer[nameof(options.ModelBindingMessageProvider.UnknownValueIsInvalidAccessor), x]);//UnknownValueIsInvalidAccessor "The supplied value is invalid for {0}."

                options.ModelBindingMessageProvider.SetValueIsInvalidAccessor((x) =>
                    localizer[nameof(options.ModelBindingMessageProvider.ValueIsInvalidAccessor), x]);//ValueIsInvalidAccessor "The value '{0}' is invalid."

                options.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((x) =>
                    localizer[nameof(options.ModelBindingMessageProvider.ValueMustBeANumberAccessor), x]);//ValueMustBeANumberAccessor "The field {0} must be a number."

                options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor((x) =>
                    localizer[nameof(options.ModelBindingMessageProvider.ValueMustNotBeNullAccessor), x]);//ValueMustNotBeNullAccessor "The value '{0}' is invalid."
            }
        }
    }
}
