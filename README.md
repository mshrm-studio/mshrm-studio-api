
<center>
    <img width=100px; height=100px; src="https://github.com/mshrm-studio/mshrm-studio-api/assets/7746153/27332cff-48d9-4c8f-93a1-928bfdd4dfc7">
</center>

# Mshrm.Studio 

> `The Mshrm.Studio Microservices project` is a our flagship application to showcase our work and explain what we can do, built with .Net Core and different software architecture and technologies like **Microservices Architecture**, **Domain Driven Design (DDD)**, **CQRS**, **Event Driven Architecture**. For communication between independent services, we use asynchronous messaging using Dapr [Dapr]([https://github.com/dapr](https://github.com/dapr)) on top of the rabbitmq library. For aggregator calls we use synchronous communication for real-time communications with using REST calls.

💡 This application mainly focuses on the technical. I have used a number of different technologies, software architecture designs, principles while creating this microservices app so this is a showcase of what we are offering. 

> **Warning**
> This project is in progress. I add new features over the time. You can check the [Release Notes (TBC.)](TBC.).

# ⭐ Support

[![Gitter](https://img.shields.io/static/v1?style=for-the-badge&message=Mshrm%20Studio%20Gitter&color=222222&logo=Gitter&logoColor=FFAE33&label=)](https://app.gitter.im/#/room/#mshrmstudio:gitter.im)

# Table of Contents

- [Features](#features)
- [Plan](#plan)
  - [Todos](#todo)
- [Setup](#setup)
  - [Dev Certificate](#dev-certificate)
  - [Analyzers](#analyzers)
- [Technologies - Libraries](#technologies---libraries)
- [Application Architecture](#application-architecture)
- [Application Structure](#application-structure)
- [Prerequisites](#prerequisites)
- [How to Run](#how-to-run)
  - [Using Docker-Compose](#using-docker-compose)
- [License](#license)

## Features

- 🔩 `Microservices` as a high level architecture
- 🔩 `Api Aggregator` to handle all incoming requests
- 🔩 `Event Driven Architecture` Dapr on top of RabbitMQ Message Broker
- 🔩 `Domain Driven Design`
- 🔩 `Data Centeric Architecture` based on `CRUD`
- 🔩 `Domain Events` using the `MediatR` library
- 🔩 `CQRS` using the `MediatR` library (only in the microservices for now)
- 🔩 `Provider Pattern` for all 3rd party API integrations
- 🔩 `UnitTests`
- 🔩 `Localization` for all error responses and are dynamic as a microservice
- 🔩 `SQL Server` for data read and write as a relational DB
- 🔩 `Redis Cache` for caching data
- 🔩 `docker and docker-compose` for deployment
- 🔩 Using `Helm`, `Kubernetes` and `Kustomize` for deployment

## Plan

> This project is in progress and this will be updated periodically.

### Todos

Items to add/update (yet to be implemented):

- Add `Structured logging` with serilog and exporting logs to `Elastic Seacrch` and `Kibana` through [serilog-sinks-elasticsearch](https://github.com/serilog-contrib/serilog-sinks-elasticsearch) sink

## Technologies - Libraries

- ✔️ **[`.NET 8`](https://dotnet.microsoft.com/download)** - .NET Framework and .NET Core, including ASP.NET and ASP.NET Core
- ✔️ **[`Dapr`](https://github.com/dapr)** - Dapr is a portable, event-driven, runtime for building distributed applications across the cloud and edge for .NET
- ✔️ **[`StackExchange.Redis`](https://github.com/StackExchange/StackExchange.Redis)** - General purpose redis client for caching
- ✔️ **[`SQL Server Entity Framework Core Provider`](https://learn.microsoft.com/en-us/ef/core/providers/sql-server/?tabs=dotnet-core-cli)** - SQL Server has an Entity Framework (EF) Core provider.
- ✔️ **[`AWSSDK.S3`]([https://learn.microsoft.com/en-us/ef/core/providers/sql-server/?tabs=dotnet-core-cli](https://github.com/aws/aws-sdk-net))** - For file storage using AWS S3 (in our case Digital Ocean)
- ✔️ **[`Mediatr`](https://github.com/jbogard/MediatR)** - A simple in process messaging library for events + CQRS [see here](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/microservice-application-layer-implementation-web-api)
- ✔️ **[`Swagger & Swagger UI`](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)** - Swagger tools for documenting API's built on ASP.NET Core
- ✔️ **[`Newtonsoft.Json`](https://github.com/JamesNK/Newtonsoft.Json)** - Json.NET is a popular high-performance JSON framework for .NET
- ✔️ **[`Hangfire`](https://github.com/HangfireIO/Hangfire)** - Background code execution using jobs for .NET
- ✔️ **[`Microsoft.Extensions.Localization`]([https://github.com/HangfireIO/Hangfire](https://learn.microsoft.com/en-us/dotnet/core/extensions/localization))** - Localization framework for .NET
- ✔️ **[`AspNetCore.Diagnostics.HealthChecks`](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks)** - Enterprise HealthChecks for ASP.NET Core Diagnostics Package
- ✔️ **[`Microsoft.AspNetCore.Authentication.JwtBearer`](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer)** - Handling Jwt Authentication and authorization in .Net Core
- ✔️ **[`StyleCopAnalyzers`](https://github.com/DotNetAnalyzers/StyleCopAnalyzers)** - An implementation of StyleCop rules using the .NET Compiler Platform
- ✔️ **[`AutoMapper`](https://github.com/AutoMapper/AutoMapper)** - Convention-based object-object mapper in .NET.
- ✔️ **[`Hellang.Middleware.ProblemDetails`](https://github.com/khellang/Middleware/tree/master/src/ProblemDetails)** - A middleware for handling exception in .Net Core
- ✔️ **[`NSwag`](https://github.com/RicoSuter/NSwag)** - For building the gateway/aggregator clients (for talking with microservices) in .NET.

## Setup

### Dev Certificate (Windows)

This application uses `Https` so to run this locally, we have to trust the dev certs created by Visual Studio. If youre not using Visual Studio (creates a pfx for you), you can use below

```powershell
dotnet dev-certs https --clean
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p <CREDENTIAL_PLACEHOLDER>
dotnet dev-certs https --trust
```

Otherwise

```powershell
dotnet dev-certs https --trust
```

### Analyzers

Stylecop can be found in the `.editorconfig` file:

- [StyleCop/StyleCop](https://github.com/StyleCop/StyleCop)

## Application Architecture

The bellow architecture shows that there is 2 public APIs which are accessible for clients and are done via HTTP request/response. 

- The Aggregator API takes an HTTP request and aggregates data to/from to the corresponding microservice/s. The HTTP request is received by microservices that act as their own REST APIs. Each microservice is running within its own `Domain` and ONLY has direct access to its own databases, files, local transaction, etc. All these dependencies are only accessible for that microservice and not to the outside world. The microservices are decoupled from each other and are autonomous and therefore do not not rely on other parts in the system. This means they can run independently of other services.
- The Authorization API handles token generation and user management. The microservice is running within its own `Domain` and ONLY has direct access to its own databases, files, local transaction, etc. All these dependencies are only accessible for that microservice and not to the outside world.

<center>
    <img src="https://github.com/mshrm-studio/mshrm-studio-api/assets/7746153/394b14fd-baf9-47ab-b47a-75ca91a4bc26">
</center>

Microservices are event based which means they can publish and/or subscribe to any events occurring in the setup using Dapr (exmaple below). By using this approach for communicating between services, each microservice does not need to know about the other services or handle errors occurred in other microservices. Dpar is used for this.

```csharp
await _daprClient.PublishEventAsync("pubsub", "send-email", new EmailDto() { EmailType = EmailType.PasswordReset, Link = "", ToEmailAddress = "test" }, cancellationToken);
```

The aggregator needs to know about all of the microservices it needs to communicate with. For this I have chosen to use NSwag which allows me to autogenerate clients with all the endpoints required for each microservice using the open API definition generated on msbuild. There is a Powersheel script included in the solution items of this project [here](https://github.com/mshrm-studio/mshrm-studio-api/blob/main/Mshrm.Studio/GenerateClients.ps1). Each microservice that requires a client to be generated must have an nswag.json file like can be found [here](https://github.com/mshrm-studio/mshrm-studio-api/blob/main/Mshrm.Studio/Mshrm.Studio.Domain.Api/nswag.json) in the domain microservice which are a list of the settings used. A breakdown of the script can be seen below.

```powershell
# Set any env vars required for application
$env:ASPNETCORE_ENVIRONMENT = 'Development';

# Get the solution folder (the parent directory of where this file lives)
$solutionFolder = (get-item $MyInvocation.MyCommand.Path).Directory.FullName

# Get all projects to run EXCEPT the aggregator/shared library
$projectsToRun = Get-ChildItem $solutionFolder -Recurse -Filter '*.Api.csproj' | Where-Object {$_.Name -notmatch 'Mshrm.Studio.Api.csproj'} | Where-Object {$_.Name -notmatch 'Mshrm.Studio.Shared.csproj'}

# Build projects
$build = Invoke-MsBuild -Path (Get-ChildItem $solutionFolder -Recurse -Filter '*.sln')[0].FullName

# For each project, generate the clients
foreach ($project in $projectsToRun)
{
   Write-Host "Running: " + $($project)
   cd $project.Directory
   Invoke-Expression -Command "nswag run /runtime:Net80"
}
```

The clients are outputted to [here](https://github.com/mshrm-studio/mshrm-studio-api/tree/main/Mshrm.Studio/Mshrm.Studio.Api/Clients)

There are some Hangfire jobs that are run when the pricing application has started which can be found [here](https://github.com/mshrm-studio/mshrm-studio-api/blob/main/Mshrm.Studio/Mshrm.Studio.Pricing.Api/Extensions/JobConfigurationExtensions.cs). These import prices from all the providers configured for the currencies that exist in the database.

## Application Structure

<center>
    <img width=800px; height=400px; src="https://github.com/mshrm-studio/mshrm-studio-api/assets/7746153/01a5b371-94e2-4407-8f20-0e1c6027d52a">
</center>

Each micro service lives in its own folder and consists of 4 projects (following DDD)

### The presentation layer (Mshrm.Studio.X.Api - Web API)

This is the API in which our aggregator interacts with. It contains controllers and code to setup/support the running of the API

### The application layer (Mshrm.Studio.X.Application - Class library)

This contains the handlers/services and application related models (Dtos)

### The domain layer (Mshrm.Studio.X.Domain - Class library)

This contains the entities/aggregate roots as well as houses buisiness logic for the application

### The infrastructure layer (Mshrm.Studio.X.Infrastructure - Class library)

This is the persistance layer that allows communication with a database, houses migrations etc.

### Adding Migrations
add-migration <migration_name>

Flow:

- API endpoint (Controller)
- Request Input (Dto)
- Request Output (Dto)
- Some class to handle Request, For example Command and Command Handler or Query and Query Handler
- Data Model

## Prerequisites

1. This application uses `Https` for hosting apis, to setup a valid certificate on your machine, you can create a [Self-Signed Certificate](https://learn.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-7.0#macos-or-linux), see more about enforce certificate [here](https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl).
2. Install git - [https://git-scm.com/downloads](https://git-scm.com/downloads).
3. Install .NET Core 8.0 - [https://dotnet.microsoft.com/download/dotnet/7.0](https://dotnet.microsoft.com/download/dotnet/8.0).
4. Install Docker
5. Install WSL2
6. Install Docker Compose - [https://docs.docker.com/docker-for-windows/install/](https://docs.docker.com/docker-for-windows/install/).
7. Install Docker Desktop - [https://docs.docker.com/docker-for-windows/install/](https://docs.docker.com/docker-for-windows/install/).
8. Clone Project [https://github.com/mshrm-studio/mshrm-studio-api.git](https://github.com/mshrm-studio/mshrm-studio-api.git)
9. Run Docker Desktop
10. Open solution.

## How to Run

For Running this application we are using [Docker-Compose](#using-docker-compose) and must have Docker desktop working.

TBC.

### Using Docker-Compose

TBC.

### Deployment

kompose --file Mshrm.Studio/docker-compose.yml --out ./k8s convert

## License

The project is under [MIT license](https://github.com/mshrm-studio/mshrm-studio-api/blob/main/LICENSE).
