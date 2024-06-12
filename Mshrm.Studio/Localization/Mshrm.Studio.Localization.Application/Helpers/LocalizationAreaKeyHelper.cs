using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Mshrm.Studio.Localization.Api.Models.Enums;
using Mshrm.Studio.Shared.Enums;
using Mshrm.Studio.Shared.Exceptions.HttpAction;

namespace Mshrm.Studio.Localization.Application.Helpers
{
    public class LocalizationAreaKeyHelper
    {
        public static List<string> GetKeys(LocalizationArea localizationArea)
        {
            var keys = new List<string>();

            switch (localizationArea)
            {
                case LocalizationArea.Errors:

                    var failureCodes = Enum.GetNames(typeof(FailureCode)).ToList();
                    keys.AddRange(failureCodes);

                    var badRequestAccessors = typeof(ModelBindingMessageProvider).GetProperties()
                        .Select(p => p.Name)
                        .Where(x => x.Contains("Accessor"))
                        .ToList();
                    keys.AddRange(badRequestAccessors);

                    break;
                default: throw new UnprocessableEntityException("Localization area not supported", FailureCode.LocalizationAreaNotSupported);
            }

            return keys;
        }
    }
}
