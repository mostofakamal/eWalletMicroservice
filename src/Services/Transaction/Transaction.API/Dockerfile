#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Transaction/Transaction.API/Transaction.API.csproj", "Services/Transaction/Transaction.API/"]
COPY ["Services/Transaction/Transaction.Domain/Transaction.Domain.csproj", "Services/Transaction/Transaction.Domain/"]
COPY ["Services/Transaction/Transaction.Infrastructure/Transaction.Infrastructure.csproj", "Services/Transaction/Transaction.Infrastructure/"]
COPY ["CoreLib/Core.Lib/Core.Lib.csproj", "CoreLib/Core.Lib/"]
RUN dotnet restore "Services/Transaction/Transaction.API/Transaction.API.csproj"
COPY . .
WORKDIR "/src/Services/Transaction/Transaction.API"
RUN dotnet build "Transaction.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Transaction.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Transaction.API.dll"]