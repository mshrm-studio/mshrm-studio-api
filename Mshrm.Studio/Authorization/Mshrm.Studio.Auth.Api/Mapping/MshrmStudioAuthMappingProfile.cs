using AutoMapper;
using Mshrm.Studio.Auth.Api.Models.Dtos;
using Mshrm.Studio.Auth.Api.Models.Entities;
using Mshrm.Studio.Auth.Api.Models.Pocos;
using Mshrm.Studio.Auth.Domain.Tokens.Commands;
using Mshrm.Studio.Auth.Domain.User.Commands;

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
        }
    }
}
