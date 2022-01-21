#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Azure.Services.Interaction.API/Azure.Services.Interaction.API.csproj", "Azure.Services.Interaction.API/"]
RUN dotnet restore "Azure.Services.Interaction.API/Azure.Services.Interaction.API.csproj"
COPY . .
WORKDIR "/src/Azure.Services.Interaction.API"
RUN dotnet build "Azure.Services.Interaction.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Azure.Services.Interaction.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Azure.Services.Interaction.API.dll"]