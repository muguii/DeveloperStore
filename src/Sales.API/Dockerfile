FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /app

COPY ["Sales.API/Sales.API.csproj", "Sales.API/"]
COPY ["Sales.Application/Sales.Application.csproj", "Sales.Application/"]
COPY ["Sales.Infrastructure/Sales.Infrastructure.csproj", "Sales.Infrastructure/"]
COPY ["Sales.Domain/Sales.Domain.csproj", "Sales.Domain/"]
COPY ["*.sln", "./"]

RUN dotnet restore "Sales.API/Sales.API.csproj"

COPY . .

WORKDIR "/app/Sales.API"

RUN dotnet build "Sales.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release

RUN dotnet publish "Sales.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sales.API.dll"]