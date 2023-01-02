#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_ENVIRONMENT Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["web.api/web.api.csproj", "web.api/"]
COPY ["infrustruture/infrustruture.csproj", "infrustruture/"]
RUN dotnet restore "web.api/web.api.csproj"
COPY . .
WORKDIR "/src/web.api"
RUN dotnet build "web.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "web.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

RUN rm /app/publish/appsettings.Test.json /app/publish/appsettings.json   #remove others and left appsetting.Development.json



FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "web.api.dll"]