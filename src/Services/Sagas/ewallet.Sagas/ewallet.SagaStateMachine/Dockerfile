#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Services/Sagas/ewallet.Sagas/ewallet.SagaStateMachine/ewallet.SagaStateMachine.csproj", "Services/Sagas/ewallet.Sagas/ewallet.SagaStateMachine/"]
COPY ["Services/Sagas/ewallet.Sagas/wallet.sagas/wallet.sagas.core.csproj", "Services/Sagas/ewallet.Sagas/wallet.sagas/"]
COPY ["CoreLib/Core.Lib/Core.Lib.csproj", "CoreLib/Core.Lib/"]
RUN dotnet restore "Services/Sagas/ewallet.Sagas/ewallet.SagaStateMachine/ewallet.SagaStateMachine.csproj"
COPY . .
WORKDIR "/src/Services/Sagas/ewallet.Sagas/ewallet.SagaStateMachine"
RUN dotnet build "ewallet.SagaStateMachine.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ewallet.SagaStateMachine.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ewallet.SagaStateMachine.dll"]