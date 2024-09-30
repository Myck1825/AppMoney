# Use the official ASP.NET Core runtime image as a base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy the published output of the ASP.NET Core application into the container
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["./AppMoney.Model.csproj", "AppMoney.Model/"]
RUN dotnet restore "./AppMoney.Model/AppMoney.Model.csproj"
COPY . .
WORKDIR "/src/AppMoney.Model"
RUN dotnet build "./AppMoney.Model.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AppMoney.Model.csproj" -c $BUILD_CONFIGURATION -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppMoney.Model.dll"]