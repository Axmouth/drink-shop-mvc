#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["asp.net-core-mvc-drink-shop/asp-net-core-mvc-drink-shop.csproj", "asp.net-core-mvc-drink-shop/"]
RUN dotnet restore "./asp.net-core-mvc-drink-shop/asp-net-core-mvc-drink-shop.csproj"
COPY . .
WORKDIR "/src/asp.net-core-mvc-drink-shop"
RUN dotnet build "asp-net-core-mvc-drink-shop.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "asp-net-core-mvc-drink-shop.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir -p /var/lib/axmouth/asp-drinks-mvc/static-assets
ENTRYPOINT ["dotnet", "asp-net-core-mvc-drink-shop.dll"]