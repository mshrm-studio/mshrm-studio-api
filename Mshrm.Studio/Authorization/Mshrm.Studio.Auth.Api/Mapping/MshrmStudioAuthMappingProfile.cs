using AutoMapper;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.Models;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Models.Pocos;
using Mshrm.Studio.Auth.Application.Dtos.ApiResources;
using Mshrm.Studio.Auth.Application.Dtos.Clients;
using Mshrm.Studio.Auth.Application.Dtos.Users;
using Mshrm.Studio.Auth.Domain.ApiResources.Commands;
using Mshrm.Studio.Auth.Domain.Clients;
using Mshrm.Studio.Auth.Domain.Clients.Commands;
using Mshrm.Studio.Auth.Domain.Clients.Enums;
using Mshrm.Studio.Auth.Domain.Tokens.Commands;
using Mshrm.Studio.Auth.Domain.User.Commands;
using Mshrm.Studio.Shared.Models.Dtos;
using Mshrm.Studio.Shared.Models.Pagination;
using ApiScope = Duende.IdentityServer.EntityFramework.Entities.ApiScope;
using Client = Duende.IdentityServer.EntityFramework.Entities.Client;
using Token = Mshrm.Studio.Auth.Api.Models.Entities.Token;

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

            CreateMap<Client, ClientResponseDto>()
                .ForMember(dest => dest.AllowedGrantTypes, src => src.MapFrom(x => MapGrantTypes(x.AllowedGrantTypes)))
            .ReverseMap();
            CreateMap<ClientWithSecret, CreatedClientResponseDto>().ReverseMap();
            CreateMap<ClientSecret, ClientSecretResponseDto>().ReverseMap();
            CreateMap<CreateClientRequestDto, CreateClientCommand>().ReverseMap();
            CreateMap<PagedResult<Client>, PageResultDto<ClientResponseDto>>();

            #endregion


            #region Api Resource

            CreateMap<ApiScope, ApiScopeResponseDto>().ReverseMap();
            CreateMap<ApiScopeClaim, ApiScopeUserClaimResponseDto>().ReverseMap();
            CreateMap<CreateApiScopeRequestDto, CreateApiScopeCommand>().ReverseMap();
            CreateMap<PagedResult<ApiScope>, PageResultDto<ApiScopeResponseDto>>();

            #endregion
        }

        private List<AllowedGrantType> MapGrantTypes(List<ClientGrantType> allowedGrantTypes)
        {
            return allowedGrantTypes.Select(x => {
                switch (x.GrantType)
                {
                    case GrantType.ClientCredentials:
                        return AllowedGrantType.ClientCredentials;
                    case GrantType.Hybrid:
                        return AllowedGrantType.Hybrid;
                    case GrantType.AuthorizationCode:
                        return AllowedGrantType.AuthorizationCode;
                    case GrantType.ResourceOwnerPassword:
                        return AllowedGrantType.ResourceOwnerPassword;
                    default: throw new NotImplementedException($"Grant type {x.GrantType} not mapped");
                };
            }).ToList();
        }
    }
}
