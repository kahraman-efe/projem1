FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY WebApplication4/WebApplication4.csproj WebApplication4/
RUN dotnet restore WebApplication4/WebApplication4.csproj

COPY WebApplication4/. WebApplication4/
WORKDIR /src/WebApplication4
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "WebApplication4.dll"]