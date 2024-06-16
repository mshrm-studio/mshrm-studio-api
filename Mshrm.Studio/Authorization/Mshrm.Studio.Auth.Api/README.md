# Mshrm.Studio Auth API

## User Secrets

dotnet user-secrets init
dotnet user-secrets set "HangfireDatabaseUsername" "username" --project Authorization\Mshrm.Studio.Auth.Api
dotnet user-secrets set "HangfireDatabasePassword" "password" --project Authorization\Mshrm.Studio.Auth.Api
dotnet user-secrets set "HangfireDatabaseEndpoint" "endpoint" --project Authorization\Mshrm.Studio.Auth.Api
dotnet user-secrets set "ApplicationDatabaseUsername" "username" --project Authorization\Mshrm.Studio.Auth.Api
dotnet user-secrets set "ApplicationDatabasePassword" "password" --project Authorization\Mshrm.Studio.Auth.Api
dotnet user-secrets set "ApplicationDatabaseEndpoint" "endpoint" --project Authorization\Mshrm.Studio.Auth.Api
dotnet user-secrets set "OpenId:MicrosoftClientId" "" --project Authorization\Mshrm.Studio.Auth.Api
dotnet user-secrets set "OpenId:MicrosoftClientSecret" "" --project Authorization\Mshrm.Studio.Auth.Api
dotnet user-secrets set "IdentityServerLicenceKey" "" --project Authorization\Mshrm.Studio.Auth.Api

## Migrations

1. Set startup project as the API ie. Mshrm.Studio.X.Api
2. Set package manager console default project to Infrastructure project ie. Mshrm.Studio.X.Infrastructure
3. Run "add-migration Migration_Name" in package manager console

Add-Migration InitialIdentityMigration -c MshrmStudioDbContext -o Migrations
Add-Migration InitialConfigurationMigration -c ConfigurationDbContext -o Migrations/IdentityServer/ConfigurationDb
Add-Migration InitialPersistedGranMigration -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb

## License

The project is under [MIT license](https://github.com/mshrm-studio/mshrm-studio-api/blob/main/LICENSE).
