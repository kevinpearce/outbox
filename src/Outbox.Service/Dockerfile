FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "./"]
COPY ["Outbox.Service/Outbox.Service.csproj", "Outbox.Service/"]
COPY ["Outbox.Common/Outbox.Common.csproj", "Outbox.Common/"]
COPY ["Outbox.Application/Outbox.Application.csproj", "Outbox.Application/"]
COPY ["Outbox.Domain/Outbox.Domain.csproj", "Outbox.Domain/"]
COPY ["Outbox.Infrastructure/Outbox.Infrastructure.csproj", "Outbox.Infrastructure/"]
RUN dotnet restore "Outbox.Service/Outbox.Service.csproj"
COPY . .
WORKDIR "/src/Outbox.Service"
RUN dotnet build "Outbox.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Outbox.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Outbox.Service.dll"]
