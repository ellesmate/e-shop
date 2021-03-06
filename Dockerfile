# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY Shop.UI/*.csproj ./Shop.UI/
COPY Shop.Application.Tests/*.csproj ./Shop.Application.Tests/
COPY Shop.Application/*.csproj ./Shop.Application/
COPY Shop.Database/*.csproj ./Shop.Database/
COPY Shop.Domain/*.csproj ./Shop.Domain/
COPY Shop.S3/*.csproj ./Shop.S3/
RUN dotnet restore

# copy everything else and build app
COPY Shop.UI/. ./Shop.UI/
COPY Shop.Application/. ./Shop.Application/
COPY Shop.Application/. ./Shop.Application.Tests/
COPY Shop.Database/. ./Shop.Database/
COPY Shop.Domain/. ./Shop.Domain/
COPY Shop.S3/. ./Shop.S3/

WORKDIR /source/Shop.UI
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
ENV ASPNETCORE_URLS=http://*:$PORT
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Shop.UI.dll"]
