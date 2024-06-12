using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Localization.Domain.LocalizationResources.Queries
{
    public class GetSupportedLocalizationResourceCulturesQuery : IRequest<List<string>>
    {
    }
}
