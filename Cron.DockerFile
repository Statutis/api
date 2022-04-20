FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Statutis.Cron/Statutis.Cron.csproj", "Statutis.Cron/"]
RUN dotnet restore "Statutis.Cron/Statutis.Cron.csproj"
COPY . .
WORKDIR "/src/Statutis.Cron"
RUN dotnet build "Statutis.Cron.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Statutis.Cron.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Statutis.Cron.dll"]
