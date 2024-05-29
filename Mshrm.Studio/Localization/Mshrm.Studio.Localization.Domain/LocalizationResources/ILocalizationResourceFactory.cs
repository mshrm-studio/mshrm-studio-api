using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Localization.Domain.LocalizationResources
{
    public interface ILocalizationResourceFactory
    {
        /// <summary>
        /// Create a new localization resource
        /// </summary>
        /// <param name="localizationArea">The are the resource is applied to</param>
        /// <param name="culture">The culture for the resource</param>
        /// <param name="name">The value to change</param>
        /// <param name="value">The value to update the name to</param>
        /// <param name="comment">A comment for the resource</param>
        /// <returns>A new localization resource</returns>
        public LocalizationResource CreateLocalizationResouce(LocalizationArea localizationArea, string culture, string name, string value, string? comment);
    }
}
