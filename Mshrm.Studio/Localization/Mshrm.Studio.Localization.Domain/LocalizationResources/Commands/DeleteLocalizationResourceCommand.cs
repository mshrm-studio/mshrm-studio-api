using MediatR;
using Mshrm.Studio.Localization.Api.Models.Entities;

namespace Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Commands
{
    public class DeleteLocalizationResourceCommand : IRequest<bool>
    {
        public Guid GuidId { get; set; }
    }
}
