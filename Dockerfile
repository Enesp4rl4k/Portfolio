# Use the .NET 9 SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY Portfolio/MyPortfolio.csproj Portfolio/
RUN dotnet restore Portfolio/MyPortfolio.csproj

# Copy the rest of the source code
COPY . .

# Build and publish the application
WORKDIR /src/Portfolio
RUN dotnet publish -c Release -o /app/publish

# Use the .NET 9 ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose port 10000 (Render's default)
EXPOSE 10000
ENV ASPNETCORE_URLS=http://+:10000

ENTRYPOINT ["dotnet", "MyPortfolio.dll"]
