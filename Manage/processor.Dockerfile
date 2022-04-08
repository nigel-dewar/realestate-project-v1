#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Manage.Listings.Processor/Manage.Listings.Processor.csproj", "Manage.Listings.Processor/"]
COPY ["Manage.Repository/Manage.Repository.csproj", "Manage.Repository/"]
COPY ["MessageBus/MessageBus.csproj", "MessageBus/"]
RUN dotnet restore "Manage.Listings.Processor/Manage.Listings.Processor.csproj"
COPY . .
WORKDIR "/src/Manage.Listings.Processor"
RUN dotnet build "Manage.Listings.Processor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Manage.Listings.Processor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Manage.Listings.Processor.dll"]
