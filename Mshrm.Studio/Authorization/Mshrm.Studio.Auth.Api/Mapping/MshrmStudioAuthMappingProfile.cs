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

            CreateMap<Token, TokenDto>().ReverseMap();

            #endregion

            #region User

            CreateMap<MshrmStudioUser, IdentityUserDto>().ReverseMap();
            CreateMap<MshrmStudioIdentityUser, IdentityUserDto>()
                .ForMember(dest => dest.Confirmed, src => src.MapFrom(x => x.EmailConfirmed))
                .ReverseMap();

            CreateMap<LoginDto, CreateTokenCommand>().ReverseMap();
            CreateMap<RefreshTokenDto, CreateRefreshTokenCommand>().ReverseMap();
            CreateMap<UpdatePasswordDto, UpdatePasswordCommand>().ReverseMap();
            CreateMap<PasswordResetTokenRequestDto, CreatePasswordResetTokenCommand>().ReverseMap();
            CreateMap<PasswordResetDto, ResetPasswordCommand>().ReverseMap();
            CreateMap<ValidateConfirmationDto, ValidateUserConfirmationCommand>().ReverseMap();
            CreateMap<ResendConfirmationDto, ResendUserConfirmationCommand>().ReverseMap();
            CreateMap<CreateUserAnyRoleDto, CreateUserAnyRoleCommand>().ReverseMap();
            CreateMap<CreateUserDto, CreateUserCommand>().ReverseMap();
   
            #endregion
        }
    }
}
