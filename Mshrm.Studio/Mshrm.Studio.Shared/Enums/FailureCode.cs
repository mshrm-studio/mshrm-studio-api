using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mshrm.Studio.Shared.Enums
{
    public enum FailureCode
    {
        UserAlreadyExists = 0,
        FailedToCreateIdentityUser = 1,
        EmailIsInvalid = 2,
        UserLockedOut = 3,
        FailedToGenerateToken = 4,
        SystemError = 5,
        IpNotValid = 6,
        CannotViewOtherUsersData = 7,
        UserDoesntExist = 8,
        FailedToRefreshToken = 9,
        NoEmailTypeRegistered = 10,
        FailedToSendEmail = 11,
        FailedToUpdatePassword = 12,
        FailedToGenerateResetToken = 13,
        FailedToGenerateConfirmationToken = 14,
        FailedToValidateConfirmationToken = 15,
        UserRequiresConfirmation = 16,
        AlreadyConfirmed = 17,
        ContactFormDoesntExist = 18,
        RoleDoesntExist = 19,
        AssetAlreadyExists = 20,
        AssetNotSupported = 21,
        KeyNotProvided = 22,
        TemporaryFileNotFound = 23,
        FileNotFound = 24,
        ResourceDoesntExist = 25,
        EmailNotFound = 26,
        FailedToCreateDomainUser = 27,
        ResourceIsPrivate = 28,
        BaseAssetNotSupported = 29,
        BaseAssetPriceDoesntExist = 30,
        AssetDoesntExist = 31,
        ToolDoesntExist = 32,
        LocalizationResourceAlreadyExists = 33,
        LocalizationResourceDoesntExist = 34,
        PricingStructureDoesntExist = 35,
    }
}
