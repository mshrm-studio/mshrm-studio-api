﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mshrm.Studio.Localization.Api.Contexts;

#nullable disable

namespace Mshrm.Studio.Localization.Api.Migrations
{
    [DbContext(typeof(MshrmStudioLocalizationDbContext))]
    partial class MshrmStudioLocalizationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Mshrm.Studio.Localization.Api.Models.Entities.LocalizationResource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Comment")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Culture")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GuidId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocalizationArea")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("LocalizationResources", "dbo");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Culture = "en-US",
                            GuidId = new Guid("c80b4365-0000-0000-0000-000000000000"),
                            Key = "RoleDoesntExist",
                            LocalizationArea = "Errors",
                            Value = "Role doesn't exist"
                        },
                        new
                        {
                            Id = 2,
                            Culture = "en-US",
                            GuidId = new Guid("0b679c15-0000-0000-0000-000000000000"),
                            Key = "UserDoesntExist",
                            LocalizationArea = "Errors",
                            Value = "User doesn't exist"
                        },
                        new
                        {
                            Id = 3,
                            Culture = "en-US",
                            GuidId = new Guid("8d33705c-0000-0000-0000-000000000000"),
                            Key = "IpNotValid",
                            LocalizationArea = "Errors",
                            Value = "The IP address provided is not valid"
                        },
                        new
                        {
                            Id = 4,
                            Culture = "en-US",
                            GuidId = new Guid("f640a108-0000-0000-0000-000000000000"),
                            Key = "EmailNotFound",
                            LocalizationArea = "Errors",
                            Value = "Email doesn't exist"
                        },
                        new
                        {
                            Id = 5,
                            Culture = "en-US",
                            GuidId = new Guid("ba1bff0f-0000-0000-0000-000000000000"),
                            Key = "FailedToCreateIdentityUser",
                            LocalizationArea = "Errors",
                            Value = "Failed to create identity user"
                        },
                        new
                        {
                            Id = 6,
                            Culture = "en-US",
                            GuidId = new Guid("02abbd52-0000-0000-0000-000000000000"),
                            Key = "FailedToCreateDomainUser",
                            LocalizationArea = "Errors",
                            Value = "Failed to create domain user"
                        },
                        new
                        {
                            Id = 7,
                            Culture = "en-US",
                            GuidId = new Guid("f561060a-0000-0000-0000-000000000000"),
                            Key = "CannotViewOtherUsersData",
                            LocalizationArea = "Errors",
                            Value = "Non admin users can only see their own data"
                        },
                        new
                        {
                            Id = 8,
                            Culture = "en-US",
                            GuidId = new Guid("4701cff9-0000-0000-0000-000000000000"),
                            Key = "EmailIsInvalid Email",
                            LocalizationArea = "Errors",
                            Value = "is invalid"
                        },
                        new
                        {
                            Id = 9,
                            Culture = "en-US",
                            GuidId = new Guid("f73424dc-0000-0000-0000-000000000000"),
                            Key = "FailedToGenerateToken",
                            LocalizationArea = "Errors",
                            Value = "Failed to generate token"
                        },
                        new
                        {
                            Id = 10,
                            Culture = "en-US",
                            GuidId = new Guid("2b7127c9-0000-0000-0000-000000000000"),
                            Key = "UserLockedOut",
                            LocalizationArea = "Errors",
                            Value = "User is locked out"
                        },
                        new
                        {
                            Id = 11,
                            Culture = "en-US",
                            GuidId = new Guid("25c1fa8e-0000-0000-0000-000000000000"),
                            Key = "UserRequiresConfirmation",
                            LocalizationArea = "Errors",
                            Value = "User requires confirmation"
                        },
                        new
                        {
                            Id = 12,
                            Culture = "en-US",
                            GuidId = new Guid("56844f06-0000-0000-0000-000000000000"),
                            Key = "FailedToRefreshToken",
                            LocalizationArea = "Errors",
                            Value = "Failed to refresh token"
                        },
                        new
                        {
                            Id = 13,
                            Culture = "en-US",
                            GuidId = new Guid("35239610-0000-0000-0000-000000000000"),
                            Key = "UserAlreadyExists",
                            LocalizationArea = "Errors",
                            Value = "User already exists"
                        },
                        new
                        {
                            Id = 14,
                            Culture = "en-US",
                            GuidId = new Guid("cdb5cbb1-0000-0000-0000-000000000000"),
                            Key = "FailedToGenerateConfirmationToken",
                            LocalizationArea = "Errors",
                            Value = "Failed to generate confirmation token"
                        },
                        new
                        {
                            Id = 15,
                            Culture = "en-US",
                            GuidId = new Guid("f561060a-0000-0000-0000-000000000000"),
                            Key = "CannotViewOtherUsersData",
                            LocalizationArea = "Errors",
                            Value = "Non admin users can only see their own data"
                        },
                        new
                        {
                            Id = 16,
                            Culture = "en-US",
                            GuidId = new Guid("d4abe423-0000-0000-0000-000000000000"),
                            Key = "FailedToGenerateResetToken",
                            LocalizationArea = "Errors",
                            Value = "Password could not be reset"
                        },
                        new
                        {
                            Id = 17,
                            Culture = "en-US",
                            GuidId = new Guid("a21ca1ed-0000-0000-0000-000000000000"),
                            Key = "FailedToUpdatePassword",
                            LocalizationArea = "Errors",
                            Value = "Failed to update password"
                        },
                        new
                        {
                            Id = 18,
                            Culture = "en-US",
                            GuidId = new Guid("11810ecf-0000-0000-0000-000000000000"),
                            Key = "AlreadyConfirmed",
                            LocalizationArea = "Errors",
                            Value = "Already confirmed"
                        },
                        new
                        {
                            Id = 19,
                            Culture = "en-US",
                            GuidId = new Guid("07a699ba-0000-0000-0000-000000000000"),
                            Key = "FailedToValidateConfirmationToken",
                            LocalizationArea = "Errors",
                            Value = "Failed to validate confirmation token"
                        },
                        new
                        {
                            Id = 20,
                            Culture = "en-US",
                            GuidId = new Guid("e776fda1-0000-0000-0000-000000000000"),
                            Key = "ContactFormDoesntExist",
                            LocalizationArea = "Errors",
                            Value = "Contact form doesn't exist"
                        },
                        new
                        {
                            Id = 21,
                            Culture = "en-US",
                            GuidId = new Guid("5d9035f0-0000-0000-0000-000000000000"),
                            Key = "ToolDoesntExist",
                            LocalizationArea = "Errors",
                            Value = "Tool doesn't exist"
                        },
                        new
                        {
                            Id = 22,
                            Culture = "en-US",
                            GuidId = new Guid("edc08f05-0000-0000-0000-000000000000"),
                            Key = "FailedToSendEmail",
                            LocalizationArea = "Errors",
                            Value = "Failed to send email"
                        },
                        new
                        {
                            Id = 23,
                            Culture = "en-US",
                            GuidId = new Guid("661f05f7-0000-0000-0000-000000000000"),
                            Key = "NoEmailTypeRegistered",
                            LocalizationArea = "Errors",
                            Value = "No email type has been registered for the email being sent"
                        },
                        new
                        {
                            Id = 24,
                            Culture = "en-US",
                            GuidId = new Guid("00204d90-0000-0000-0000-000000000000"),
                            Key = "LocalizationResourceAlreadyExists",
                            LocalizationArea = "Errors",
                            Value = "Localization resource already exists"
                        },
                        new
                        {
                            Id = 25,
                            Culture = "en-US",
                            GuidId = new Guid("1ee8505e-0000-0000-0000-000000000000"),
                            Key = "LocalizationResourceDoesntExist",
                            LocalizationArea = "Errors",
                            Value = "Localization resource doesn't exist"
                        },
                        new
                        {
                            Id = 26,
                            Culture = "en-US",
                            GuidId = new Guid("1640b96e-0000-0000-0000-000000000000"),
                            Key = "AssetAlreadyExists",
                            LocalizationArea = "Errors",
                            Value = "The asset already exists"
                        },
                        new
                        {
                            Id = 27,
                            Culture = "en-US",
                            GuidId = new Guid("b023770e-0000-0000-0000-000000000000"),
                            Key = "AssetNotSupported",
                            LocalizationArea = "Errors",
                            Value = "Asset is not supported by the provider"
                        },
                        new
                        {
                            Id = 28,
                            Culture = "en-US",
                            GuidId = new Guid("752cd5be-0000-0000-0000-000000000000"),
                            Key = "BaseAssetNotSupported",
                            LocalizationArea = "Errors",
                            Value = "The base asset symbol provided is not supported"
                        },
                        new
                        {
                            Id = 29,
                            Culture = "en-US",
                            GuidId = new Guid("f1198411-0000-0000-0000-000000000000"),
                            Key = "BaseAssetPriceDoesntExist",
                            LocalizationArea = "Errors",
                            Value = "The base asset symbols price doesn't exist"
                        },
                        new
                        {
                            Id = 30,
                            Culture = "en-US",
                            GuidId = new Guid("61cab136-0000-0000-0000-000000000000"),
                            Key = "AssetDoesntExist",
                            LocalizationArea = "Errors",
                            Value = "The asset doesn't exists"
                        },
                        new
                        {
                            Id = 31,
                            Culture = "en-US",
                            GuidId = new Guid("b4bbfdf3-0000-0000-0000-000000000000"),
                            Key = "TemporaryFileNotFound",
                            LocalizationArea = "Errors",
                            Value = "Temporary file not found"
                        },
                        new
                        {
                            Id = 32,
                            Culture = "en-US",
                            GuidId = new Guid("6a6d907c-0000-0000-0000-000000000000"),
                            Key = "KeyNotProvided",
                            LocalizationArea = "Errors",
                            Value = "Key must be provided"
                        },
                        new
                        {
                            Id = 33,
                            Culture = "en-US",
                            GuidId = new Guid("3eb6f092-0000-0000-0000-000000000000"),
                            Key = "FileNotFound",
                            LocalizationArea = "Errors",
                            Value = "File not found"
                        },
                        new
                        {
                            Id = 34,
                            Culture = "en-US",
                            GuidId = new Guid("d2b9aad8-0000-0000-0000-000000000000"),
                            Key = "ResourceDoesntExist",
                            LocalizationArea = "Errors",
                            Value = "Resource doesn't exist"
                        },
                        new
                        {
                            Id = 35,
                            Culture = "en-US",
                            GuidId = new Guid("40b76499-0000-0000-0000-000000000000"),
                            Key = "ResourceIsPrivate",
                            LocalizationArea = "Errors",
                            Value = "Cannot view this resource unauthenticated"
                        },
                        new
                        {
                            Id = 36,
                            Culture = "en-US",
                            GuidId = new Guid("31c8a568-0000-0000-0000-000000000000"),
                            Key = "SystemError",
                            LocalizationArea = "Errors",
                            Value = "System error, please contact admin"
                        },
                        new
                        {
                            Id = 37,
                            Culture = "en-US",
                            GuidId = new Guid("3650fb28-0000-0000-0000-000000000000"),
                            Key = "PricingStructureDoesntExist",
                            LocalizationArea = "Errors",
                            Value = "The pricing structure doesn't exist"
                        },
                        new
                        {
                            Id = 38,
                            Culture = "en-US",
                            GuidId = new Guid("75348680-0000-0000-0000-000000000000"),
                            Key = "AssetPriceMustBeACurrency",
                            LocalizationArea = "Errors",
                            Value = "The asset price must be a currency"
                        },
                        new
                        {
                            Id = 39,
                            Culture = "en-US",
                            GuidId = new Guid("bfb8760e-0000-0000-0000-000000000000"),
                            Key = "FailureCodeIsInvalid",
                            LocalizationArea = "Errors",
                            Value = "The failure code provided is invalid"
                        },
                        new
                        {
                            Id = 40,
                            Culture = "en-US",
                            GuidId = new Guid("86337cda-0000-0000-0000-000000000000"),
                            Key = "LocalizationAreaNotSupported",
                            LocalizationArea = "Errors",
                            Value = "The localization area provided is not supported"
                        },
                        new
                        {
                            Id = 41,
                            Culture = "en-US",
                            GuidId = new Guid("911ff394-0000-0000-0000-000000000000"),
                            Key = "CultureNotSupported",
                            LocalizationArea = "Errors",
                            Value = "The culture provided is not supported"
                        },
                        new
                        {
                            Id = 42,
                            Culture = "en-US",
                            GuidId = new Guid("e3a248b8-0000-0000-0000-000000000000"),
                            Key = "AttemptedValueIsInvalidAccessor",
                            LocalizationArea = "Errors",
                            Value = "The value '{0}' is not valid for {1}."
                        },
                        new
                        {
                            Id = 43,
                            Culture = "en-US",
                            GuidId = new Guid("9870e536-0000-0000-0000-000000000000"),
                            Key = "MissingBindRequiredValueAccessor",
                            LocalizationArea = "Errors",
                            Value = "A value for the '{0}' parameter or property was not provided."
                        },
                        new
                        {
                            Id = 44,
                            Culture = "en-US",
                            GuidId = new Guid("133ff005-0000-0000-0000-000000000000"),
                            Key = "MissingKeyOrValueAccessor",
                            LocalizationArea = "Errors",
                            Value = "A value is required."
                        },
                        new
                        {
                            Id = 45,
                            Culture = "en-US",
                            GuidId = new Guid("5f075f3e-0000-0000-0000-000000000000"),
                            Key = "MissingRequestBodyRequiredValueAccessor",
                            LocalizationArea = "Errors",
                            Value = "A non-empty request body is required."
                        },
                        new
                        {
                            Id = 46,
                            Culture = "en-US",
                            GuidId = new Guid("8a59242e-0000-0000-0000-000000000000"),
                            Key = "NonPropertyAttemptedValueIsInvalidAccessor",
                            LocalizationArea = "Errors",
                            Value = "The value '{0}' is not valid."
                        },
                        new
                        {
                            Id = 47,
                            Culture = "en-US",
                            GuidId = new Guid("4e0dc373-0000-0000-0000-000000000000"),
                            Key = "NonPropertyUnknownValueIsInvalidAccessor",
                            LocalizationArea = "Errors",
                            Value = "The supplied value is invalid."
                        },
                        new
                        {
                            Id = 48,
                            Culture = "en-US",
                            GuidId = new Guid("eaaff39c-0000-0000-0000-000000000000"),
                            Key = "NonPropertyValueMustBeANumberAccessor",
                            LocalizationArea = "Errors",
                            Value = "The field must be a number."
                        },
                        new
                        {
                            Id = 49,
                            Culture = "en-US",
                            GuidId = new Guid("791f3028-0000-0000-0000-000000000000"),
                            Key = "UnknownValueIsInvalidAccessor",
                            LocalizationArea = "Errors",
                            Value = "The supplied value is invalid for {0}."
                        },
                        new
                        {
                            Id = 50,
                            Culture = "en-US",
                            GuidId = new Guid("d3102189-0000-0000-0000-000000000000"),
                            Key = "ValueIsInvalidAccessor",
                            LocalizationArea = "Errors",
                            Value = "The value '{0}' is invalid."
                        },
                        new
                        {
                            Id = 51,
                            Culture = "en-US",
                            GuidId = new Guid("491dac58-0000-0000-0000-000000000000"),
                            Key = "ValueMustBeANumberAccessor",
                            LocalizationArea = "Errors",
                            Value = "The field {0} must be a number."
                        },
                        new
                        {
                            Id = 52,
                            Culture = "en-US",
                            GuidId = new Guid("a27a477b-0000-0000-0000-000000000000"),
                            Key = "ValueMustNotBeNullAccessor",
                            LocalizationArea = "Errors",
                            Value = "The value '{0}' is invalid."
                        },
                        new
                        {
                            Id = 53,
                            Culture = "en-US",
                            GuidId = new Guid("f446c6e6-0000-0000-0000-000000000000"),
                            Key = "LocalizationAreaKeyIsInvalid",
                            LocalizationArea = "Errors",
                            Value = "The key provided for the localization area is invalid."
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
