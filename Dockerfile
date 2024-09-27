#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

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
COPY ["AppMoney.Database/AppMoney.Database.csproj", "AppMoney.Database/"]
COPY ["AppMoney.Response/AppMoney.Response.csproj", "AppMoney.Response/"]
COPY ["AppMoney.Model/AppMoney.Model.csproj", "AppMoney.Model/"]
COPY ["AppMoney.Worker/AppMoney.Worker.csproj", "AppMoney.Worker/"]
RUN dotnet restore "AppMoney.sln"

COPY . .

RUN dotnet build "AppMoney.sln" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AppMoney.sln" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppMoney.dll"]