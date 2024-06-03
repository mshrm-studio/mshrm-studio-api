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
ARG CompanyEmail
ARG CompanyEmailPassword
ARG JwtSigningKey
ENV ApplicationDatabaseUsername=${ApplicationDatabaseUsername}
ENV ApplicationDatabasePassword=${ApplicationDatabasePassword}
ENV HangfireDatabaseUsername=${HangfireDatabaseUsername}
ENV HangfireDatabasePassword=${HangfireDatabasePassword}
ENV Email:CompanyEmail=${CompanyEmail}
ENV Email:CompanyEmailPassword=${CompanyEmailPassword}
ENV Jwt:JwtSigningKey=${JwtSigningKey}

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./Mshrm.Studio.Email.Api/Mshrm.Studio.Email.Api.csproj", "Mshrm.Studio.Email.Api/"]
COPY ["./Mshrm.Studio.Email.Application/Mshrm.Studio.Email.Application.csproj", "Mshrm.Studio.Email.Application/"]
COPY ["./Mshrm.Studio.Email.Domain/Mshrm.Studio.Email.Domain.csproj", "Mshrm.Studio.Email.Domain/"]
COPY ["./Mshrm.Studio.Email.Infrastructure/Mshrm.Studio.Email.Infrastructure.csproj", "Mshrm.Studio.Email.Infrastructure/"]
COPY ./nuget.config ./
RUN dotnet restore "./Mshrm.Studio.Email.Api/Mshrm.Studio.Email.Api.csproj"
WORKDIR "/src/Mshrm.Studio.Email.Api"
COPY . .
RUN dotnet build "./Mshrm.Studio.Email.Api/Mshrm.Studio.Email.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Mshrm.Studio.Email.Api/Mshrm.Studio.Email.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Mshrm.Studio.Email.Api.dll"]