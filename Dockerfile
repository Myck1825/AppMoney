FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["AppMoney.sln", "./"]
COPY ["AppMoney/AppMoney.csproj", "AppMoney/"]
RUN dotnet restore "./AppMoney/AppMoney.csproj"

COPY . .

WORKDIR /src/AppMoney
RUN dotnet build "./AppMoney.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish
RUN dotnet publish "./AppMoney.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppMoney.dll"]