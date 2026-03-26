FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["src/services/AuthService/AuthService.Domain/AuthService.Domain.csproj", "src/services/AuthService/AuthService.Domain/"]
COPY ["src/services/AuthService/AuthService.Application/AuthService.Application.csproj", "src/services/AuthService/AuthService.Application/"]
COPY ["src/services/AuthService/AuthService.Infrastructure/AuthService.Infrastructure.csproj", "src/services/AuthService/AuthService.Infrastructure/"]
COPY ["src/services/AuthService/AuthService.API/AuthService.API.csproj", "src/services/AuthService/AuthService.API/"]

RUN dotnet restore "src/services/AuthService/AuthService.API/AuthService.API.csproj"

COPY . .

RUN dotnet publish "src/services/AuthService/AuthService.API/AuthService.API.csproj" -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "AuthService.API.dll"]