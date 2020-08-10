#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Kyc.API/Kyc.API.csproj", "Kyc.API/"]
RUN dotnet restore "Kyc.API/Kyc.API.csproj"
COPY . .
WORKDIR "/src/Kyc.API"
RUN dotnet build "Kyc.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kyc.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kyc.API.dll"]