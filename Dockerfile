FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["blog-api-dev.csproj", "./"]
RUN dotnet restore "blog-api-dev.csproj"
COPY . .
RUN dotnet build "blog-api-dev.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "blog-api-dev.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# ENTRYPOINT ["dotnet", "blog-api-dev.dll"]
RUN useradd -m grandemestre
USER grandemestre
CMD ASPNETCORE_URLS="http://*:$PORT" dotnet blog-api-dev.dll