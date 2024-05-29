using AutoMapper;
using Mshrm.Studio.Localization.Api.Models.CQRS.LocalizationResources.Commands;
using Mshrm.Studio.Localization.Api.Models.Dtos.LocalizationResources;
using Mshrm.Studio.Localization.Api.Models.Entities;

namespace Mshrm.Studio.Localization.Api.Mapping
{
    /// <summary>
    /// The localization mapping profile
    /// </summary>
    public class MshrmStudioLocalizationMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MshrmStudioLocalizationMappingProfile()
        {
            Init();
        }

        /// <summary>
        /// Init mappings
        /// </summary>
        public void Init()
        {
            #region Localization Resource

            CreateMap<LocalizationResource, LocalizationResourceDto>().ReverseMap();
            CreateMap<CreateLocalizationResourceDto, CreateLocalizationResourceCommand>().ReverseMap();

            #endregion
        }
    }
}
