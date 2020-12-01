FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build-env
WORKDIR /app
EXPOSE 443

COPY ["SE.Web/SE.Web.csproj", "SE.Web/"]
COPY ["SE.Business/SE.Business.csproj", "SE.Business/"]
COPY ["SE.Core/SE.Core.csproj", "SE.Core/"]
COPY ["SE.Data/SE.Data.csproj", "SE.Data/"]

RUN dotnet restore "SE.Web/SE.Web.csproj"

COPY . ./
RUN dotnet publish "SE.Web/SE.Web.csproj" -c Release -o /app/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "SE.Web.dll"]
