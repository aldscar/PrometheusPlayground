#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MeasuredWebApplication/MeasuredWebApplication.csproj", "MeasuredWebApplication/"]
RUN dotnet restore "MeasuredWebApplication/MeasuredWebApplication.csproj"
COPY . .
WORKDIR "/src/MeasuredWebApplication"
RUN dotnet build "MeasuredWebApplication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MeasuredWebApplication.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MeasuredWebApplication.dll"]
EXPOSE 80