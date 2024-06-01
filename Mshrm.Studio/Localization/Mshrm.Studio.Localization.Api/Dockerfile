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
COPY ["./Mshrm.Studio.Localization.Api/Mshrm.Studio.Localization.Api.csproj", "Mshrm.Studio.Localization.Api/"]
COPY ["./Mshrm.Studio.Localization.Application/Mshrm.Studio.Localization.Application.csproj", "Mshrm.Studio.Localization.Application/"]
COPY ["./Mshrm.Studio.Localization.Domain/Mshrm.Studio.Localization.Domain.csproj", "Mshrm.Studio.Localization.Domain/"]
COPY ["./Mshrm.Studio.Localization.Infrastructure/Mshrm.Studio.Localization.Infrastructure.csproj", "Mshrm.Studio.Localization.Infrastructure/"]
COPY ./nuget.config ./
RUN dotnet restore "./Mshrm.Studio.Localization.Api/Mshrm.Studio.Localization.Api.csproj"
WORKDIR "/src/Mshrm.Studio.Localization.Api"
COPY . .
RUN dotnet build "./Mshrm.Studio.Localization.Api/Mshrm.Studio.Localization.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mshrm.Studio.Localization.Api/Mshrm.Studio.Localization.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mshrm.Studio.Localization.Api.dll"]