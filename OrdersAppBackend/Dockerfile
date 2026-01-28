# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY OrdersAppBackend/OrdersAppBackend.csproj ./OrdersAppBackend/
COPY OrdersAppBackend.Tests/OrdersAppBackend.Tests.csproj ./OrdersAppBackend.Tests/
RUN dotnet restore OrdersAppBackend/OrdersAppBackend.csproj
RUN dotnet restore OrdersAppBackend.Tests/OrdersAppBackend.Tests.csproj

COPY OrdersAppBackend/ ./OrdersAppBackend/
COPY OrdersAppBackend.Tests/ ./OrdersAppBackend.Tests/

# Test stage - executes tests and fails the build if tests fail
FROM build AS test
WORKDIR /src
RUN dotnet test OrdersAppBackend.Tests/OrdersAppBackend.Tests.csproj --configuration Release --logger "console;verbosity=detailed"

# Publish stage - only runs if tests pass
FROM build AS publish
WORKDIR /src/OrdersAppBackend
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "OrdersAppBackend.dll"]
