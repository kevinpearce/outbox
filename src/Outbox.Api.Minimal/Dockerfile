FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "./"]
COPY ["Outbox.Api.Minimal/Outbox.Api.Minimal.csproj", "Outbox.Api.Minimal/"]
COPY ["Outbox.Domain/Outbox.Domain.csproj", "Outbox.Domain/"]
COPY ["Outbox.Application/Outbox.Application.csproj", "Outbox.Application/"]
COPY ["Outbox.Common/Outbox.Common.csproj", "Outbox.Common/"]
COPY ["Outbox.Infrastructure/Outbox.Infrastructure.csproj", "Outbox.Infrastructure/"]
RUN dotnet restore "Outbox.Api.Minimal/Outbox.Api.Minimal.csproj"
COPY . .
WORKDIR "/src/Outbox.Api.Minimal"
RUN dotnet build "Outbox.Api.Minimal.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Outbox.Api.Minimal.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Outbox.Api.Minimal.dll"]
