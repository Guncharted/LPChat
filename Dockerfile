#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["LPChat.API/LPChat.API.csproj", "LPChat.API/"]
COPY ["LPChat.Services/LPChat.Services.csproj", "LPChat.Services/"]
COPY ["LPChat.Common/LPChat.Common.csproj", "LPChat.Common/"]
COPY ["LPChat.Services.Abstractions/LPChat.Services.Abstractions.csproj", "LPChat.Services.Abstractions/"]
COPY ["LPChat.Data.MongoDb.Abstractions/LPChat.Data.MongoDb.Abstractions.csproj", "LPChat.Data.MongoDb.Abstractions/"]
COPY ["LPChat.Data.MongoDb/LPChat.Data.MongoDb.csproj", "LPChat.Data.MongoDb/"]
RUN dotnet restore "LPChat.API/LPChat.API.csproj"
COPY . .
WORKDIR "/src/LPChat.API"
RUN dotnet build "LPChat.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LPChat.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LPChat.API.dll"]