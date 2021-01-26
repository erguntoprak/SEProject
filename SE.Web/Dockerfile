FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env

WORKDIR /app


COPY ["SE.Web/SE.Web.csproj", "SE.Web/"]

COPY ["SE.Business/SE.Business.csproj", "SE.Business/"]

COPY ["SE.Core/SE.Core.csproj", "SE.Core/"]

COPY ["SE.Data/SE.Data.csproj", "SE.Data/"]



RUN dotnet restore "SE.Web/SE.Web.csproj"

COPY . ./

RUN dotnet publish "SE.Web/SE.Web.csproj" -c Release -o out



# Build runtime image

FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "SE.Web.dll"]
