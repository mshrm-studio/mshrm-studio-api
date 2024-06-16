#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ARG ApplicationDatabaseUsername
ARG ApplicationDatabasePassword
ARG JwtSigningKey
ARG IdentityServerLicenceKey
ENV ApplicationDatabaseUsername=${ApplicationDatabaseUsername}
ENV ApplicationDatabasePassword=${ApplicationDatabasePassword}
ENV Jwt:JwtSigningKey=${JwtSigningKey}
ENV IdentityServerLicenceKey=${IdentityServerLicenceKey}

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Mshrm.Studio.Auth.Api/Mshrm.Studio.Auth.Api.csproj", "Mshrm.Studio.Auth.Api/"]
COPY ["./Mshrm.Studio.Auth.Application/Mshrm.Studio.Auth.Application.csproj", "Mshrm.Studio.Auth.Application/"]
COPY ["./Mshrm.Studio.Auth.Domain/Mshrm.Studio.Auth.Domain.csproj", "Mshrm.Studio.Auth.Domain/"]
COPY ["./Mshrm.Studio.Auth.Infrastructure/Mshrm.Studio.Auth.Infrastructure.csproj", "Mshrm.Studio.Auth.Infrastructure/"]
COPY ./nuget.config ./
RUN dotnet restore "./Mshrm.Studio.Auth.Api/Mshrm.Studio.Auth.Api.csproj" --configfile nuget.config 
WORKDIR "/src/Mshrm.Studio.Auth.Api"
COPY . .
RUN dotnet build "./Mshrm.Studio.Auth.Api/Mshrm.Studio.Auth.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mshrm.Studio.Auth.Api/Mshrm.Studio.Auth.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mshrm.Studio.Auth.Api.dll"]