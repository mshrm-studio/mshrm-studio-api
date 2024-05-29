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
COPY ["./Mshrm.Studio.Domain.Api/Mshrm.Studio.Domain.Api.csproj", "Mshrm.Studio.Domain.Api/"]
COPY ["./Mshrm.Studio.Domain.Application/Mshrm.Studio.Domain.Application.csproj", "Mshrm.Studio.Domain.Application/"]
COPY ["./Mshrm.Studio.Domain.Domain/Mshrm.Studio.Domain.Domain.csproj", "Mshrm.Studio.Domain.Domain/"]
COPY ["./Mshrm.Studio.Domain.Infrastructure/Mshrm.Studio.Domain.Infrastructure.csproj", "Mshrm.Studio.Domain.Infrastructure/"]
COPY ./nuget.config ./
RUN dotnet restore "./Mshrm.Studio.Domain.Api/Mshrm.Studio.Domain.Api.csproj"
WORKDIR "/src/Mshrm.Studio.Domain.Api"
COPY . .
RUN dotnet build "./Mshrm.Studio.Domain.Api/Mshrm.Studio.Domain.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build 

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mshrm.Studio.Domain.Api/Mshrm.Studio.Domain.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mshrm.Studio.Domain.Api.dll"]