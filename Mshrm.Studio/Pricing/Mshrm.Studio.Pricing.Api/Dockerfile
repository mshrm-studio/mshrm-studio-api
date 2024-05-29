#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ARG ApplicationDatabaseUsername
ARG ApplicationDatabasePassword
ARG HangfireDatabaseUsername
ARG HangfireDatabasePassword
ARG JwtSigningKey
ENV ApplicationDatabaseUsername=${ApplicationDatabaseUsername}
ENV ApplicationDatabasePassword=${ApplicationDatabasePassword}
ENV HangfireDatabaseUsername=${HangfireDatabaseUsername}
ENV HangfireDatabasePassword=${HangfireDatabasePassword}
ENV Jwt:JwtSigningKey=${JwtSigningKey}

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Mshrm.Studio.Pricing.Api/Mshrm.Studio.Pricing.Api.csproj", "Mshrm.Studio.Pricing.Api/"]
COPY ["./Mshrm.Studio.Pricing.Application/Mshrm.Studio.Pricing.Application.csproj", "Mshrm.Studio.Pricing.Application/"]
COPY ["./Mshrm.Studio.Pricing.Domain/Mshrm.Studio.Pricing.Domain.csproj", "Mshrm.Studio.Pricing.Domain/"]
COPY ["./Mshrm.Studio.Pricing.Infrastructure/Mshrm.Studio.Pricing.Infrastructure.csproj", "Mshrm.Studio.Pricing.Infrastructure/"]
COPY ./nuget.config ./
RUN dotnet restore "./Mshrm.Studio.Pricing.Api/Mshrm.Studio.Pricing.Api.csproj"
WORKDIR "/src/Mshrm.Studio.Pricing.Api"
COPY . .
RUN dotnet build "./Mshrm.Studio.Pricing.Api/Mshrm.Studio.Pricing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mshrm.Studio.Pricing.Api/Mshrm.Studio.Pricing.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mshrm.Studio.Pricing.Api.dll"]