FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["assignment_3_movievault.csproj", "./"]
RUN dotnet restore "assignment_3_movievault.csproj"

COPY . .
RUN dotnet publish "assignment_3_movievault.csproj" \
    --configuration Release \
    --output /app/publish \
    --no-restore \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
RUN apt-get update \
    && apt-get install --no-install-recommends -y curl \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8000
EXPOSE 8000

ENTRYPOINT ["dotnet", "assignment_3_movievault.dll"]
