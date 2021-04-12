#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["GreenFluxAssignment.Api/GreenFluxAssignment.Api.csproj", "GreenFluxAssignment.Api/"]
COPY ["GreenFluxAssignment.Domain/GreenFluxAssignment.Domain.csproj", "GreenFluxAssignment.Domain/"]
COPY ["GreenFluxAssignment.Api.Contracts/GreenFluxAssignment.Api.Contracts.csproj", "GreenFluxAssignment.Api.Contracts/"]
COPY ["GreenFluxAssignment.Persistence/GreenFluxAssignment.Persistence.csproj", "GreenFluxAssignment.Persistence/"]
RUN dotnet restore "GreenFluxAssignment.Api/GreenFluxAssignment.Api.csproj"
COPY . .
WORKDIR "/src/GreenFluxAssignment.Api"
RUN dotnet build "GreenFluxAssignment.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GreenFluxAssignment.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GreenFluxAssignment.Api.dll"]