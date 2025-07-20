# Use official .NET SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and projects
COPY ../Summer-Bookstore.sln ./
COPY ../Summer-Bookstore/*.csproj ./Summer-Bookstore/
COPY ../Summer-Bookstore-Domain/*.csproj ./Summer-Bookstore-Domain/
COPY ../Summer-Bookstore-Infrastructure/*.csproj ./Summer-Bookstore-Infrastructure/
COPY ../Summer_Bookstore.Application/*.csproj ./Summer_Bookstore.Application/

# Restore dependencies
RUN dotnet restore ./Summer-Bookstore/Summer-Bookstore.csproj

# Copy everything else
COPY ../ ./

# Build the app
RUN dotnet publish ./Summer-Bookstore/Summer-Bookstore.csproj -c Release -o /app/publish

# Use runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Expose port
EXPOSE 80

# Start the app
ENTRYPOINT ["dotnet", "Summer-Bookstore.dll"]
