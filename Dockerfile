FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only the project files first, so the restore step is cached
COPY Burgers.Core/Burgers.Core.csproj Burgers.Core/
COPY Burgers.Data/Burgers.Data.csproj Burgers.Data/
COPY Burgers.Api/Burgers.Api.csproj Burgers.Api/
RUN dotnet restore Burgers.Api/Burgers.Api.csproj

COPY Burgers.Core/ Burgers.Core/
COPY Burgers.Data/ Burgers.Data/
COPY Burgers.Api/ Burgers.Api/
RUN dotnet publish Burgers.Api/Burgers.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Burgers.Api.dll"]