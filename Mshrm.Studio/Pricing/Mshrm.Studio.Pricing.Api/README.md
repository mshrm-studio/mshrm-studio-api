# Mshrm.Studio Pricing API

## User Secrets

dotnet user-secrets init
dotnet user-secrets set "HangfireDatabaseUsername" "username" --project Pricing\Mshrm.Studio.Pricing.Api
dotnet user-secrets set "HangfireDatabasePassword" "password" --project Pricing\Mshrm.Studio.Pricing.Api
dotnet user-secrets set "ApplicationDatabaseUsername" "username" --project Pricing\Mshrm.Studio.Pricing.Api
dotnet user-secrets set "ApplicationDatabasePassword" "password" --project Pricing\Mshrm.Studio.Pricing.Api
dotnet user-secrets set "TwelveDataProvider:ApiKey" "apikey" --project Pricing\Mshrm.Studio.Pricing.Api
dotnet user-secrets set "FreeCurrencyService:ApiKey" "apikey" --project Pricing\Mshrm.Studio.Pricing.Api
dotnet user-secrets set "MobulaService:ApiKey" "apikey" --project Pricing\Mshrm.Studio.Pricing.Api
dotnet user-secrets set "PolygonIOService:ApiKey" "apikey" --project Pricing\Mshrm.Studio.Pricing.Api
dotnet user-secrets set "MetalsDevService:ApiKey" "apikey" --project Pricing\Mshrm.Studio.Pricing.Api


## Migrations

1. Set startup project as the API ie. Mshrm.Studio.X.Api
2. Set package manager console default project to Infrastructure project ie. Mshrm.Studio.X.Infrastructure
3. Run "add-migration Migration_Name" in package manager console

## License

The project is under [MIT license](https://github.com/mshrm-studio/mshrm-studio-api/blob/main/LICENSE).

