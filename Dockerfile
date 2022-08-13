#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["AplaTabApi/AplaTabApi.csproj", "AplaTabApi/"]
RUN dotnet restore "AplaTabApi/AplaTabApi.csproj"
COPY . .
WORKDIR "/src/AplaTabApi"
RUN dotnet build "AplaTabApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AplaTabApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AplaTabApi.dll"]