
<center>
    <img width=100px; height=100px; src="https://github.com/mshrm-studio/mshrm-studio-api/assets/7746153/27332cff-48d9-4c8f-93a1-928bfdd4dfc7">
</center>

# Mshrm.Studio Storage API

> `The Mshrm.Studio Storage API` is a microservice that handles uploaded files using S3 storage (Digital Ocean is used here but can be swapped out for AWS). It is hidden from the outside world but is available for aggregator calls, pub-sub using Dapr within the internal network. The project uses CQRS to manage controller actions and follows the DDD pattern to handle internal events/manage database data.

💡 This application mainly focuses on the technical. I have used a number of different technologies, software architecture designs, principles while creating this microservices app so this is a showcase of what we are offering. 

> **Warning**
> This project is in progress. I add new features over the time. You can check the [Release Notes (TBC.)](TBC.).

# ⭐ Support

[![Gitter](https://img.shields.io/static/v1?style=for-the-badge&message=Mshrm%20Studio%20Gitter&color=222222&logo=Gitter&logoColor=FFAE33&label=)](https://app.gitter.im/#/room/#mshrmstudio:gitter.im)

# Table of Contents

- [Features](#features)
- [Plan](#plan)
- [Technologies - Libraries](#technologies---libraries)
- [Application Architecture](#application-architecture)
- [Application Structure](#application-structure)
- [License](#license)

## Features

- 🔩 `Event Driven Architecture` Dapr on top of RabbitMQ Message Broker
- 🔩 `Domain Driven Design`
- 🔩 `Data Centeric Architecture` based on `CRUD`
- 🔩 `Domain Events` using the `MediatR` library
- 🔩 `CQRS` using the `MediatR` library (only in the microservices for now)
- 🔩 `UnitTests`
- 🔩 `SQL Server` for data read and write as a relational DB

## Plan

> This project is in progress and this will be updated periodically.

## Technologies - Libraries

- ✔️ **[`.NET 8`](https://dotnet.microsoft.com/download)** - .NET Framework and .NET Core, including ASP.NET and ASP.NET Core
- ✔️ **[`Dapr`](https://github.com/dapr)** - Dapr is a portable, event-driven, runtime for building distributed applications across the cloud and edge for .NET
- ✔️ **[`SQL Server Entity Framework Core Provider`](https://learn.microsoft.com/en-us/ef/core/providers/sql-server/?tabs=dotnet-core-cli)** - SQL Server has an Entity Framework (EF) Core provider.
- ✔️ **[`AWSSDK.S3`]([https://learn.microsoft.com/en-us/ef/core/providers/sql-server/?tabs=dotnet-core-cli](https://github.com/aws/aws-sdk-net))** - For file storage using AWS S3 (in our case Digital Ocean)
- ✔️ **[`Mediatr`](https://github.com/jbogard/MediatR)** - A simple in process messaging library for events + CQRS [see here](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api)
- ✔️ **[`Swagger & Swagger UI`](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)** - Swagger tools for documenting API's built on ASP.NET Core
- ✔️ **[`Newtonsoft.Json`](https://github.com/JamesNK/Newtonsoft.Json)** - Json.NET is a popular high-performance JSON framework for .NET
- ✔️ **[`Hangfire`](https://github.com/HangfireIO/Hangfire)** - Background code execution using jobs for .NET
- ✔️ **[`AspNetCore.Diagnostics.HealthChecks`](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks)** - Enterprise HealthChecks for ASP.NET Core Diagnostics Package
- ✔️ **[`Microsoft.AspNetCore.Authentication.JwtBearer`](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)** - Handling Jwt Authentication and authorization in .Net Core
- ✔️ **[`StyleCopAnalyzers`](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)** - An implementation of StyleCop rules using the .NET Compiler Platform
- ✔️ **[`AutoMapper`](https://github.com/AutoMapper/AutoMapper)** - Convention-based object-object mapper in .NET.
- ✔️ **[`Hellang.Middleware.ProblemDetails`](https://github.com/khellang/Middleware/tree/master/src/ProblemDetails)** - A middleware for handling exception in .Net Core
- ✔️ **[`NSwag`](https://github.com/RicoSuter/NSwag)** - For building the gateway/aggregator clients (for talking with microservices) in .NET.

## Application Architecture

TBC.

This microservice is event based which means it can publish and/or subscribe to any events occurring in the setup (across internal microservices) using Dapr (exmaple below). By using this approach for communicating between services, each microservice does not need to know about the other services or handle errors occurred in other microservices. Dpar is used for this.

```csharp
await _daprClient.PublishEventAsync("pubsub", "send-email", new EmailDto() { EmailType = EmailType.PasswordReset, Link = "", ToEmailAddress = "test" }, cancellationToken);
```

## Migrations

1. Set startup project as the API ie. Mshrm.Studio.X.Api
2. Set package manager console default project to Infrastructure project ie. Mshrm.Studio.X.Infrastructure
3. Run "add-migration Migration_Name" in package manager console

## Application Structure

TBC.

### Request Flow

- API endpoint (Controller)
- Request input (Dto) is mapped to a command/query (CQRS)
- Mediatr is used to resolve where the command/query goes
- Command/query is executed
- Code executed saves/updates/reteives data from database/S3 bucket
- If there is a domain event (local) attached to the action then it is fired and executed too
- If there is a pub-sub event fired (using Dapr) then this is executed asynchornously by the subscirbing service
- A response returned if intended
- This is then mapped to a request Output (Dto) if something is returned


## User Secrets

dotnet user-secrets init
dotnet user-secrets set "HangfireDatabaseUsername" "username" --project Storage\Mshrm.Studio.Storage.Api
dotnet user-secrets set "HangfireDatabasePassword" "password" --project Storage\Mshrm.Studio.Storage.Api
dotnet user-secrets set "ApplicationDatabaseUsername" "username" --project Storage\Mshrm.Studio.Storage.Api
dotnet user-secrets set "ApplicationDatabasePassword" "password" --project Storage\Mshrm.Studio.Storage.Api
dotnet user-secrets set "DigitalOceanSpaces:Secret" "secret" --project Storage\Mshrm.Studio.Storage.Api
dotnet user-secrets set "DigitalOceanSpaces:Key" "key" --project Storage\Mshrm.Studio.Storage.Api
dotnet user-secrets set "DigitalOceanSpaces:Secret" "secret" --project Storage\Mshrm.Studio.Storage.Api

## License

The project is under [MIT license](https://github.com/mshrm-studio/mshrm-studio-api/blob/main/LICENSE).
