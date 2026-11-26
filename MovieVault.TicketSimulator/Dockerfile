FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["MovieVault.TicketSimulator.csproj", "./"]
RUN dotnet restore "MovieVault.TicketSimulator.csproj"

COPY . .
RUN dotnet publish "MovieVault.TicketSimulator.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/runtime:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "MovieVault.TicketSimulator.dll"]
