FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["./AppMoney.Database.csproj", "AppMoney.Database/"]
RUN dotnet restore "./AppMoney.Database/AppMoney.Database.csproj"

COPY . .

WORKDIR "/src/AppMoney.Database"
RUN dotnet build "./AppMoney.Database.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish
RUN dotnet publish "./AppMoney.Database.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppMoney.Database.dll"]