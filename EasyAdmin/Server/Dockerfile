#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
# RUN apt-get update && apt-get install ca-certificates #  && rm -rf /var/cache/apk/*
COPY Certificates  /etc/ssl/certs
COPY Certificates  /usr/local/share/ca-certificates
RUN update-ca-certificates
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["EasyAdmin/Server/EasyAdmin.Server.csproj", "EasyAdmin/Server/"]
COPY ["EasyAdmin/Client/EasyAdmin.Client.csproj", "EasyAdmin/Client/"]
COPY ["EasyAdmin/Shared/EasyAdmin.Shared.csproj", "EasyAdmin/Shared/"]
COPY ["EasyAdmin.Services.Ovirt/EasyAdmin.Services.Ovirt.csproj", "EasyAdmin.Services.Ovirt/"]
COPY ["EasyAdmin.Services.VMware/EasyAdmin.Services.VMware.csproj", "EasyAdmin.Services.VMware/"]
RUN dotnet restore "EasyAdmin/Server/EasyAdmin.Server.csproj"
COPY . .
WORKDIR "/src/EasyAdmin/Server"
RUN dotnet build "EasyAdmin.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EasyAdmin.Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EasyAdmin.Server.dll"]