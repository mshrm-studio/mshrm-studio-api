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
ENV ApplicationDatabaseUsername=${ApplicationDatabaseUsername}
ENV ApplicationDatabasePassword=${ApplicationDatabasePassword}
ENV HangfireDatabaseUsername=${HangfireDatabaseUsername}
ENV HangfireDatabasePassword=${HangfireDatabasePassword}

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Mshrm.Studio.Storage.Api/Mshrm.Studio.Storage.Api.csproj", "Mshrm.Studio.Storage.Api/"]
COPY ["./Mshrm.Studio.Storage.Application/Mshrm.Studio.Storage.Application.csproj", "Mshrm.Studio.Storage.Application/"]
COPY ["./Mshrm.Studio.Storage.Domain/Mshrm.Studio.Storage.Domain.csproj", "Mshrm.Studio.Storage.Domain/"]
COPY ["./Mshrm.Studio.Storage.Infrastructure/Mshrm.Studio.Storage.Infrastructure.csproj", "Mshrm.Studio.Storage.Infrastructure/"]
COPY ./nuget.config ./
RUN dotnet restore "./Mshrm.Studio.Storage.Api/Mshrm.Studio.Storage.Api.csproj"
WORKDIR "/src/Mshrm.Studio.Storage.Api"
COPY . .
RUN dotnet build "./Mshrm.Studio.Storage.Api/Mshrm.Studio.Storage.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mshrm.Studio.Storage.Api/Mshrm.Studio.Storage.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mshrm.Studio.Storage.Api.dll"]