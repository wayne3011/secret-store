# копируем базовый образ с runtime средой
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

# копируем образ с sdk
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# указываем что релизный билд будет использоваться по умолчанию
ARG BUILD_CONFIGURATION=Release
# устанавливаем папку в которой будем работать
WORKDIR /src
# копируем файл проекта в контейнер
COPY ["SecretStore.csproj", "SecretStore/"]
# восстанавливаем зависимости
RUN dotnet restore "SecretStore/SecretStore.csproj"
# переходим в каталог приложения внутри контейнера
WORKDIR "/src/SecretStore"
# копируем сюда все файлы из директории проекта с хост машины
COPY . .
# собираем проект
RUN dotnet build "SecretStore.csproj" -c $BUILD_CONFIGURATION -o /app/build

# этап публикации приложения(собираем нативное приложение, которое умеет стартовать без sdk)
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SecretStore.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# установка точки запуска
FROM base AS final
WORKDIR /app/build
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SecretStore.dll"]
