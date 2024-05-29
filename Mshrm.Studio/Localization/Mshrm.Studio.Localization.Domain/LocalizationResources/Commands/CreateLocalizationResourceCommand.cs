using MediatR;
using Mshrm.Studio.Localization.Api.Models.Entities;
using Mshrm.Studio.Localization.Api.Models.Enums;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Commands
{
    public class CreateLocalizationResourceCommand : IRequest<LocalizationResource>
    {
        /// <summary>
        /// The localization culture
        /// </summary>
        public required string Culture { get; set; }

        /// <summary>
        /// The key
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The value for key
        /// </summary>
        public required string Value { get; set; }

        /// <summary>
        /// Any comments
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// A category for localization
        /// </summary>
        public LocalizationArea LocalizationArea { get; set; }
    }
}
