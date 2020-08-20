#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Reward/Reward.API/Reward.API.csproj", "Services/Reward/Reward.API/"]
COPY ["Services/Reward/Reward.Domain/Reward.Domain.csproj", "Services/Reward/Reward.Domain/"]
COPY ["Services/Reward/Reward.Infrastructure/Reward.Infrastructure.csproj", "Services/Reward/Reward.Infrastructure/"]
COPY ["CoreLib/Core.Lib/Core.Lib.csproj", "CoreLib/Core.Lib/"]
RUN dotnet restore "Services/Reward/Reward.API/Reward.API.csproj"
COPY . .
WORKDIR "/src/Services/Reward/Reward.API"
RUN dotnet build "Reward.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Reward.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Reward.API.dll"]