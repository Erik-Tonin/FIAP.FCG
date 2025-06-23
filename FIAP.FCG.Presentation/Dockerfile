# Etapa build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY *.sln .
COPY FIAP.FCG.Presentation/*.csproj FIAP.FCG.Presentation/
COPY FIAP.FCG.Application/*.csproj FIAP.FCG.Application/
COPY FIAP.FCG.Domain/*.csproj FIAP.FCG.Domain/
COPY FIAP.FCG.Domain.Core/*.csproj FIAP.FCG.Domain.Core/
COPY FIAP.FCG.Infra/*.csproj FIAP.FCG.Infra/
COPY FIAP.FCG.Crosscutting/*.csproj FIAP.FCG.Crosscutting/
COPY FIAP.FCG.UnitTests/*.csproj FIAP.FCG.UnitTests/

RUN dotnet restore

COPY . .
WORKDIR /app/FIAP.FCG.Presentation
RUN dotnet publish -c Release -o out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/FIAP.FCG.Presentation/out .

EXPOSE 80
ENTRYPOINT ["dotnet", "FIAP.FCG.Presentation.dll"]