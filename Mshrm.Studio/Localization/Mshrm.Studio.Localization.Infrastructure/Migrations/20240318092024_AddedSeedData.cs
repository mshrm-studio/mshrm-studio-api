using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mshrm.Studio.Localization.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dbo",
                table: "LocalizationResources",
                columns: new[] { "Id", "Comment", "Culture", "GuidId", "LocalizationArea", "Name", "Value" },
                values: new object[,]
                {
                    { 1, null, "en-US", new Guid("d568e126-2f53-4674-a3c5-b1255b4f9cf5"), "Errors", "RoleDoesntExist", "Role doesn't exist" },
                    { 2, null, "en-US", new Guid("9ad6d8a8-2c0a-42bf-b580-cf25b8edd672"), "Errors", "UserDoesntExist", "User doesn't exist" },
                    { 3, null, "en-US", new Guid("3a0614a8-3fc8-4ded-a33c-892082a44412"), "Errors", "IpNotValid", "The IP address provided is not valid" },
                    { 4, null, "en-US", new Guid("085b1340-8629-427b-89c1-20321d3cd2bd"), "Errors", "EmailNotFound", "Email doesn't exist" },
                    { 5, null, "en-US", new Guid("4319e9c9-ce8b-494d-b8c2-ade6889a530b"), "Errors", "FailedToCreateIdentityUser", "Failed to create identity user" },
                    { 6, null, "en-US", new Guid("dcae985d-f716-4d69-a920-87823fed5e7e"), "Errors", "FailedToCreateDomainUser", "Failed to create domain user" },
                    { 7, null, "en-US", new Guid("ac72cfd1-2fe2-4b59-8308-26171241b61a"), "Errors", "CannotViewOtherUsersData", "Non admin users can only see their own data" },
                    { 8, null, "en-US", new Guid("5a16acf5-f794-427c-a128-8db32a254e83"), "Errors", "EmailIsInvalid Email", "is invalid" },
                    { 9, null, "en-US", new Guid("0d360a82-e513-4e3b-a204-2792154119c8"), "Errors", "FailedToGenerateToken", "Failed to generate token" },
                    { 10, null, "en-US", new Guid("1381faf1-54b9-48d4-b4a5-a843c236e3e1"), "Errors", "UserLockedOut", "User is locked out" },
                    { 11, null, "en-US", new Guid("64846125-9116-47c2-a99c-644703e72b9d"), "Errors", "UserRequiresConfirmation", "User requires confirmation" },
                    { 12, null, "en-US", new Guid("bfd59788-748c-4546-885b-8e2216baff42"), "Errors", "FailedToRefreshToken", "Failed to refresh token" },
                    { 13, null, "en-US", new Guid("007e45ae-a92b-4250-a25d-94a1449aeaee"), "Errors", "UserAlreadyExists", "User already exists" },
                    { 14, null, "en-US", new Guid("8cfed9e9-a7c0-4ca8-ba16-bc4d5cebeb11"), "Errors", "FailedToGenerateConfirmationToken", "Failed to generate confirmation token" },
                    { 15, null, "en-US", new Guid("669c5215-3d5a-409f-b746-43d2174674fb"), "Errors", "CannotViewOtherUsersData", "Non admin users can only see their own data" },
                    { 16, null, "en-US", new Guid("0c38d0bf-3bc6-424b-8c5d-fe61da7b97e1"), "Errors", "FailedToGenerateResetToken", "Password could not be reset" },
                    { 17, null, "en-US", new Guid("2e9d10c4-9d59-48df-97a2-0e60881fb7ab"), "Errors", "FailedToUpdatePassword", "Failed to update password" },
                    { 18, null, "en-US", new Guid("2578d493-804d-4174-846e-9ba72228a500"), "Errors", "AlreadyConfirmed", "Already confirmed" },
                    { 19, null, "en-US", new Guid("17cb6b93-c9c8-4d3b-b51c-3d555442ea17"), "Errors", "FailedToValidateConfirmationToken", "Failed to validate confirmation token" },
                    { 20, null, "en-US", new Guid("674ffbe9-9d39-4663-8ff7-fc0305373a26"), "Errors", "ContactFormDoesntExist", "Contact form doesn't exist" },
                    { 21, null, "en-US", new Guid("1fa13ead-1cac-4f0c-ad13-08a259e61e86"), "Errors", "ToolDoesntExist", "Tool doesn't exist" },
                    { 22, null, "en-US", new Guid("53117493-56d8-4188-a450-25297e14a25d"), "Errors", "FailedToSendEmail", "Failed to send email" },
                    { 23, null, "en-US", new Guid("bf36753c-b8c6-4edf-b483-20e15e07ffe5"), "Errors", "NoEmailTypeRegistered", "No email type has been registered for the email being sent" },
                    { 24, null, "en-US", new Guid("250b23f4-70a9-4232-b4fc-a80cbe3b1964"), "Errors", "LocalizationResourceAlreadyExists", "Localization resource already exists" },
                    { 25, null, "en-US", new Guid("6aebdbc8-c2ae-45ea-9e02-66700945813f"), "Errors", "LocalizationResourceDoesntExist", "Localization resource doesn't exist" },
                    { 26, null, "en-US", new Guid("0eadd8d6-9e1c-4a67-a863-fc712c31f25f"), "Errors", "CurrencyAlreadyExists", "The currency already exists" },
                    { 27, null, "en-US", new Guid("ebe64cac-da3e-413a-822a-4dd51bcc5cc5"), "Errors", "CurrencyNotSupported", "Currency is not supported by the provider" },
                    { 28, null, "en-US", new Guid("62b37378-0f95-41a9-8454-1bb0d87e18f5"), "Errors", "BaseCurrencyNotSupported", "The base currency symbol provided is not supported" },
                    { 29, null, "en-US", new Guid("628085ae-5b9d-488d-bf09-48ac3a39cd05"), "Errors", "BaseCurrencyPriceDoesntExist", "The base currency symbols price doesn't exist" },
                    { 30, null, "en-US", new Guid("ed07a053-7f56-41a8-a0a5-7870d593526a"), "Errors", "CurrencyDoesntExist", "The currency doesn't exists" },
                    { 31, null, "en-US", new Guid("70e0d351-0a13-4819-81df-40854fd04201"), "Errors", "TemporaryFileNotFound", "Temporary file not found" },
                    { 32, null, "en-US", new Guid("c99ad72a-2a1b-477a-ae08-38a5326fc424"), "Errors", "KeyNotProvided", "Key must be provided" },
                    { 33, null, "en-US", new Guid("82d75ed9-5270-41a8-8b81-358a421f0c4c"), "Errors", "FileNotFound", "File not found" },
                    { 34, null, "en-US", new Guid("39c7bb7c-d6c4-412b-863d-3fb743dcf8e8"), "Errors", "ResourceDoesntExist", "Resource doesn't exist" },
                    { 35, null, "en-US", new Guid("78f1f443-a0a6-4f99-9948-099e2d24a4d4"), "Errors", "ResourceIsPrivate", "Cannot view this resource unauthenticated" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "LocalizationResources",
                keyColumn: "Id",
                keyValue: 35);
        }
    }
}
