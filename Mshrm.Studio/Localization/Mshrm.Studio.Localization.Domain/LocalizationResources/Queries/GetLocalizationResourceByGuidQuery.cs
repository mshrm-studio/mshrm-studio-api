using MediatR;
using Mshrm.Studio.Localization.Api.Models.Entities;

namespace Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Queries
{
    public class GetLocalizationResourceByGuidQuery : IRequest<LocalizationResource>
    {
        public Guid GuidId { get; set; }
    }
}
