#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Crm.LinkApi/Crm.LinkApi.csproj", "Crm.LinkApi/"]
RUN dotnet restore "Crm.LinkApi/Crm.LinkApi.csproj"
COPY . .
WORKDIR "/src/Crm.LinkApi"
RUN dotnet build "Crm.LinkApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crm.LinkApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crm.LinkApi.dll"]