using MediatR;
using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Api.Models.Enums;

namespace Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Queries
{
    public class GetLocalizationResourcesQuery : IRequest<List<LocalizationResource>>
    {
        public LocalizationArea? Area { get; set; }
        public string? Culture { get; set; }
        public string? Name { get; set; }
    }
}
