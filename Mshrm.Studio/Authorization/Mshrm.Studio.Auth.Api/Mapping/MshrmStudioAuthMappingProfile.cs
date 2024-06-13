using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Models.Pocos;
using Mshrm.Studio.Auth.Application.Dtos.ApiResources;
using Mshrm.Studio.Auth.Application.Dtos.Clients;
using Mshrm.Studio.Auth.Application.Dtos.Users;
using Mshrm.Studio.Auth.Domain.ApiResources.Commands;
using Mshrm.Studio.Auth.Domain.Clients;
using Mshrm.Studio.Auth.Domain.Clients.Commands;
using Mshrm.Studio.Auth.Domain.Tokens.Commands;
using Mshrm.Studio.Auth.Domain.User.Commands;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;

namespace Mshrm.Studio.Auth.Api.Mapping
{
    /// <summary>
    /// The auth mapping profile
    /// </summary>
    public class MshrmStudioAuthMappingProfile : Profile
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MshrmStudioAuthMappingProfile()
        {
            Init();
        }

        /// <summary>
        /// Init mappings
        /// </summary>
        private void Init()
        {
            #region Token

            CreateMap<Token, TokenResponseDto>().ReverseMap();

            #endregion

            #region User

            CreateMap<MshrmStudioUser, IdentityUserResponseDto>().ReverseMap();
            CreateMap<MshrmStudioIdentityUser, IdentityUserResponseDto>()
                .ForMember(dest => dest.Confirmed, src => src.MapFrom(x => x.EmailConfirmed))
                .ReverseMap();

            CreateMap<LoginRequestDto, CreateTokenCommand>().ReverseMap();
            CreateMap<RefreshTokenRequestDto, CreateRefreshTokenCommand>().ReverseMap();
            CreateMap<UpdatePasswordRequestDto, UpdatePasswordCommand>().ReverseMap();
            CreateMap<PasswordResetTokenRequestDto, CreatePasswordResetTokenCommand>().ReverseMap();
            CreateMap<PasswordResetRequestDto, ResetPasswordCommand>().ReverseMap();
            CreateMap<ValidateConfirmationRequestDto, ValidateUserConfirmationCommand>().ReverseMap();
            CreateMap<ResendConfirmationRequestDto, ResendUserConfirmationCommand>().ReverseMap();
            CreateMap<CreateUserAnyRoleRequestDto, CreateUserAnyRoleCommand>().ReverseMap();
            CreateMap<CreateUserRequestDto, CreateUserCommand>().ReverseMap();

            #endregion

            #region Client

            CreateMap<Client, ClientResponseDto>().ReverseMap();
            CreateMap<ClientWithSecret, CreatedClientResponseDto>().ReverseMap();
            CreateMap<ClientSecret, ClientSecretResponseDto>().ReverseMap();
            CreateMap<CreateClientRequestDto, CreateClientCommand>().ReverseMap();
            CreateMap<PagedResult<Client>, PageResultDto<ClientResponseDto>>();

            #endregion


            #region Api Resource

            CreateMap<ApiScope, ApiScopeResponseDto>().ReverseMap();
            CreateMap<CreateApiScopeRequestDto, CreateApiScopeCommand>().ReverseMap();
            CreateMap<PagedResult<ApiScope>, PageResultDto<ApiScopeResponseDto>>();

            #endregion
        }
    }
}
