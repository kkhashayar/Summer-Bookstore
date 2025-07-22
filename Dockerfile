# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY Summer-Bookstore.sln ./
COPY Summer-Bookstore/*.csproj ./Summer-Bookstore/
COPY Summer-Bookstore-Domain/*.csproj ./Summer-Bookstore-Domain/
COPY Summer-Bokkstore-Infrastructure/*.csproj ./Summer-Bokkstore-Infrastructure/
COPY Summer_Bookstore.Application/*.csproj ./Summer_Bookstore.Application/

# Restore dependencies
RUN dotnet restore ./Summer-Bookstore/Summer-Bookstore-API.csproj

# Copy all source code
COPY . .

# Publish the application
RUN dotnet publish ./Summer-Bookstore/Summer-Bookstore-API.csproj -c Release -o /app/publish

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "Summer-Bookstore-API.dll"]
