# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TestMinimalApi/TestMinimalApi.csproj", "TestMinimalApi/"]
RUN dotnet restore "./TestMinimalApi/TestMinimalApi.csproj"
COPY . .
WORKDIR "/src/TestMinimalApi"
RUN dotnet build "./TestMinimalApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./TestMinimalApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Render မှ PORT ပေးပို့လာပါက ဖမ်းယူမည်၊ မရှိပါက 8080 ကို သုံးမည်
ENTRYPOINT ["sh", "-c", "dotnet TestMinimalApi.dll --urls http://0.0.0.0:${PORT:-8080}"]