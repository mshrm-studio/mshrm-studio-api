using AutoMapper;
using Mshrm.Studio.Domain.Api.Models.CQRS.ContactForms.Commands;
using Mshrm.Studio.Domain.Api.Models.CQRS.Users.Commands;
using Mshrm.Studio.Domain.Api.Models.Dtos.ContactForms;
using Mshrm.Studio.Domain.Api.Models.Dtos.Tools;
using Mshrm.Studio.Domain.Api.Models.Dtos.Users;
using Mshrm.Studio.Domain.Api.Models.Entity;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Domain.Api.Mapping
{
    /// <summary>
    /// The domain mapping profile
    /// </summary>
    public class MshrmStudioDomainMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MshrmStudioDomainMappingProfile()
        {
            Init();
        }

        /// <summary>
        /// Init domain mappings
        /// </summary>
        private void Init()
        {
            #region User

            CreateMap<User,DomainUserDto>().ReverseMap();
            CreateMap<CreateDomainUserDto, CreateUserCommand>().ReverseMap();

            #endregion

            #region Contact Forms

            CreateMap<ContactForm, ContactFormDto>().ReverseMap();
            CreateMap<PagedResult<ContactForm>, PageResultDto<ContactFormDto>>()
                .ForMember(dest => dest.PageNumber, src => src.MapFrom(x => x.Page.PageNumber))
                .ForMember(dest => dest.PerPage, src => src.MapFrom(x => x.Page.PerPage))
                .ForMember(dest => dest.Order, src => src.MapFrom(x => x.SortOrder.Order))
                .ForMember(dest => dest.PropertyName, src => src.MapFrom(x => x.SortOrder.PropertyName))
                .ReverseMap();
            CreateMap<CreateContactFormDto, CreateContactFormCommand>().ReverseMap();

            #endregion

            #region Tools

            CreateMap<Tool, ToolDto>();
            CreateMap<PagedResult<Tool>, PageResultDto<ToolDto>>().ReverseMap();
            CreateMap<CreateToolDto, CreateToolCommand>().ReverseMap();
            CreateMap<UpdateToolDto, UpdateToolCommand>().ReverseMap();

            #endregion
        }
    }
}
