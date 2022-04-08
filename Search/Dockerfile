#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Search.Api/Search.Elastic.Service.csproj", "Search.Api/"]
COPY ["Search.Api/australian_postcodes.csv", "Search.Api/australian_postcodes.csv"]
COPY ["Search.Common/Search.Common.csproj", "Search.Common/"]
COPY ["MessageBus/MessageBus.csproj", "MessageBus/"]

RUN dotnet restore "Search.Api/Search.Elastic.Service.csproj"
COPY . .
COPY ["Search.Api/australian_postcodes.csv", "Search.Api/australian_postcodes.csv"]
WORKDIR "/src/Search.Api"
RUN dotnet build "Search.Elastic.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Search.Elastic.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Search.Elastic.Service.dll"]
