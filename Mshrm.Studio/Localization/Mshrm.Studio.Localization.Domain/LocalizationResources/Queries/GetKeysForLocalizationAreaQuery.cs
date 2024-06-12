using MediatR;
using Mshrm.Studio.Localization.Api.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Localization.Domain.LocalizationResources.Queries
{
    public class GetKeysForLocalizationAreaQuery : IRequest<List<string>>
    {
        public LocalizationArea LocalizationArea { get; set; }
    }
}
