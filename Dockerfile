#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80/tcp

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LimaBooks.Api/LimaBooks.Api.csproj", "LimaBooks.Api/"]
RUN dotnet restore "LimaBooks.Api/LimaBooks.Api.csproj"
COPY . .
WORKDIR "/src/LimaBooks.Api"
RUN dotnet build "LimaBooks.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LimaBooks.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#RUN dotnet dev-certs https --verbose
ENTRYPOINT ["dotnet", "LimaBooks.Api.dll"]