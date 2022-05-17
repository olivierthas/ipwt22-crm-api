#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Crm.Link.Api/Crm.Link.Api.csproj", "Crm.Link.Api/"]
COPY ["Crm.Link.RabbitMq/Crm.Link.RabbitMq.csproj", "Crm.Link.RabbitMq/"]
COPY ["Crm.Link.Suitcrm.Tools/Crm.Link.Suitcrm.Tools.csproj", "Crm.Link.Suitcrm.Tools/"]
RUN dotnet restore "Crm.Link.Api/Crm.Link.Api.csproj"
COPY . .
WORKDIR "/src/Crm.Link.Api"
RUN dotnet build "Crm.Link.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Crm.Link.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Crm.Link.Api.dll"]