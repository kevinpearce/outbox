FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5002

FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "./"]
COPY ["Outbox.Ui/Outbox.Ui.csproj", "Outbox.Ui/"]
RUN dotnet restore "Outbox.Ui/Outbox.Ui.csproj"
COPY . .
WORKDIR "/src/Outbox.Ui"
RUN dotnet build "Outbox.Ui.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Outbox.Ui.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Outbox.Ui.dll"]
