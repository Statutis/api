FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Statutis.API/Statutis.API.csproj", "Statutis.API/"]
RUN dotnet restore "Statutis.API/Statutis.API.csproj"
COPY . .
WORKDIR "/src/Statutis.API"
RUN dotnet build "Statutis.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Statutis.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Statutis.API.dll"]
