#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443  


FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["SE.Web/SE.Web.csproj", "SE.Web/"]
COPY ["SE.Business/SE.Business.csproj", "SE.Business/"]
COPY ["SE.Core/SE.Core.csproj", "SE.Core/"]
COPY ["SE.Data/SE.Data.csproj", "SE.Data/"]
RUN dotnet restore "SE.Web/SE.Web.csproj"
COPY . .
WORKDIR "/src/SE.Web"
RUN dotnet build "SE.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SE.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SE.Web.dll"]