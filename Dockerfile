# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["BookCatalogApp.csproj", "./"]
RUN dotnet restore "BookCatalogApp.csproj"

COPY . .
RUN dotnet publish "BookCatalogApp.csproj" -c Release -o /app/publish

# Этап запуска
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

# Railway передает порт через переменную PORT
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "BookCatalogApp.dll"]