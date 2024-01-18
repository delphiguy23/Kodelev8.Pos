FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Web.Point.Of.Sale/Web.Point.Of.Sale.csproj", "Web.Point.Of.Sale/"]
RUN dotnet restore "Web.Point.Of.Sale/Web.Point.Of.Sale.csproj"
COPY . .
WORKDIR "/src/Web.Point.Of.Sale"
RUN dotnet build "Web.Point.Of.Sale.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Web.Point.Of.Sale.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.Point.Of.Sale.dll"]
