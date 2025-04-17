# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy only the necessary files for the build
COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish TaskManagerAPI.csproj -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy the built files from the build stage
COPY --from=build /app/out ./

# Set the environment variables if needed (optional)
# ENV ASPNETCORE_ENVIRONMENT=Production

# Expose the port the app will run on
EXPOSE 80

# Set entry point to run the API
ENTRYPOINT ["dotnet", "TaskManagerAPI.dll"]
