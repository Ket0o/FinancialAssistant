# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# В зависимости от операционной системы хост-компьютеров, которые будут выполнять сборку контейнеров или запускать их, может потребоваться изменить образ, указанный в инструкции FROM.
# Дополнительные сведения см. на странице https://aka.ms/containercompat

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 7015


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FinancialAssistant.Web/FinancialAssistant.Web.csproj", "FinancialAssistant.Web/"]
COPY ["FinancialAssistant.DataAccess/FinancialAssistant.DataAccess.csproj", "FinancialAssistant.DataAccess/"]
COPY ["FinancialAssistant.DataTransfer/FinancialAssistant.DataTransfer.csproj", "FinancialAssistant.DataTransfer/"]
COPY ["FinancialAssistant.EmojiService/FinancialAssistant.EmojiService.csproj", "FinancialAssistant.EmojiService/"]
COPY ["FinancialAssistant.ExchangeRates/FinancialAssistant.ExchangeRates.csproj", "FinancialAssistant.ExchangeRates/"]
COPY ["FinancialAssistant.Repository/FinancialAssistant.Repository.csproj", "FinancialAssistant.Repository/"]
RUN dotnet restore "./FinancialAssistant.Web/FinancialAssistant.Web.csproj"
COPY . .
WORKDIR "/src/FinancialAssistant.Web"
RUN dotnet build "./FinancialAssistant.Web.csproj" -c %BUILD_CONFIGURATION% -o /app/build --property WarningLevel=0

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FinancialAssistant.Web.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false --property WarningLevel=0

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
ENV ASPNETCORE_URLS="http://+:7015"
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FinancialAssistant.Web.dll"]