FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8085

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./AppMoney.Worker.csproj", "AppMoney.Worker/"]
RUN dotnet restore "./AppMoney.Worker/AppMoney.Worker.csproj"

COPY . .

WORKDIR "/src/AppMoney.Worker"
RUN dotnet build "./AppMoney.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish
RUN dotnet publish "./AppMoney.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppMoney.Worker.dll"]