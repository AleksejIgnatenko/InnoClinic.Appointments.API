# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["InnoClinic.Appointments.API/InnoClinic.Appointments.API.csproj", "InnoClinic.Appointments.API/"]
COPY ["InnoClinic.Appointments.Application/InnoClinic.Appointments.Application.csproj", "InnoClinic.Appointments.Application/"]
COPY ["InnoClinic.Appointments.Core/InnoClinic.Appointments.Core.csproj", "InnoClinic.Appointments.Core/"]
COPY ["InnoClinic.Appointments.DataAccess/InnoClinic.Appointments.DataAccess.csproj", "InnoClinic.Appointments.DataAccess/"]
COPY ["InnoClinic.Appointments.Infrastructure/InnoClinic.Appointments.Infrastructure.csproj", "InnoClinic.Appointments.Infrastructure/"]

RUN dotnet restore "InnoClinic.Appointments.API/InnoClinic.Appointments.API.csproj"

COPY . .

WORKDIR "/src/InnoClinic.Appointments.API"
RUN dotnet build "InnoClinic.Appointments.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
RUN dotnet publish "InnoClinic.Appointments.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InnoClinic.Appointments.API.dll"]