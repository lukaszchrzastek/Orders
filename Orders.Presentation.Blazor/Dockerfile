# Etap bazowy: ASP.NET Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Etap build: kompilacja projektu
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Kopiowanie plików projektu i przywracanie zale¿noœci
COPY ["Orders.Presentation.Blazor/Orders.Presentation.Blazor.csproj", "Orders.Presentation.Blazor/"]
COPY ["Orders.Infrastructure/Orders.Infrastructure.csproj", "Orders.Infrastructure/"]
COPY ["Orders.Application/Orders.Application.csproj", "Orders.Application/"]
COPY ["Orders.Domain/Orders.Domain.csproj", "Orders.Domain/"]
RUN dotnet restore "Orders.Presentation.Blazor/Orders.Presentation.Blazor.csproj"

# Kopiowanie ca³ego Ÿród³a
COPY . .
WORKDIR "/src/Orders.Presentation.Blazor"
RUN dotnet build "Orders.Presentation.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Etap publikacji
FROM build AS publish
RUN dotnet publish "Orders.Presentation.Blazor.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Etap finalny: obraz produkcyjny
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orders.Presentation.Blazor.dll"]