#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:2.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:2.1 AS build
WORKDIR /src
COPY ["SimpleBalance/SimpleBalance.csproj", "SimpleBalance/"]
RUN dotnet restore "SimpleBalance/SimpleBalance.csproj"
COPY . .
WORKDIR "/src/SimpleBalance"
RUN dotnet build "SimpleBalance.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleBalance.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleBalance.dll"]